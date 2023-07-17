using BootCamp.Core.Convertors;
using BootCamp.Core.DTOs;
using BootCamp.Core.Generator;
using BootCamp.Core.Security;
using BootCamp.Core.Services.Interfaces;
using BootCamp.DataLayer.Context;
using BootCamp.DataLayer.Entities.User;
using BootCamp.DataLayer.Entities.Wallet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootCamp.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;

        public AccountService(ApplicationDbContext context)
        {
            _context=context;
        }

        public int ChargeWallet(string userName, int amount, string description, bool isPay = true)
        {
            Wallet wallet = new Wallet()
            {
                Amount = amount,
                CreateDate = DateTime.Now,
                Description = description,
                IsPay = isPay,
                TypeId = 1,
                UserId = GetUserIdByUserName(userName)
            };
            return AddWallet(wallet);
        }

        public int AddWallet(Wallet wallet)
        {
            _context.Wallet.Add(wallet);
            _context.SaveChanges();
            return wallet.WalletId;
        }
        public void DeleteUser(int userId)
        {
            User user = GetUserById(userId);
            user.IsDelete = true;
            UpdateUser(user);
        }
        public bool ComparePassword(string userName, string password)
        {
            var hashPassword = PasswordHelper.EncodePasswordMd5(password);

            return _context.User.Any(x =>x.UserName==userName && x.Password == hashPassword);
        }

        public void EditPassword(string userName, EditProfileViewModel profile)
        {
            var user = GetUserByUserName(userName);
            user.Password = PasswordHelper.EncodePasswordMd5(profile.Password);
            _context.SaveChanges();
        }

        public void EditProfile(string userName, EditProfileViewModel profile)
        {
            if (profile.UserAvatar != null)
            {
                string imagePath = "";
                if (profile.AvatarName != "Default.jpg")
                {
                    imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", profile.AvatarName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }

                profile.AvatarName = NameGenerator.GenerateUniqCode() + Path.GetExtension(profile.UserAvatar.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", profile.AvatarName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    profile.UserAvatar.CopyTo(stream);
                }

            }

            var user = GetUserByUserName(userName);
            user.Email =FixedText.FixedEmail(profile.Email);
            user.UserName = profile.UserName;
            user.UserAvatar = profile.AvatarName;
            user.Password = PasswordHelper.EncodePasswordMd5(profile.Password);
            _context.SaveChanges();

        }

        public int GetBallanceUserWallet(string userName)
        {
            int userId=GetUserIdByUserName(userName);
            var enter=_context.Wallet.Where(w=>w.UserId== userId && w.TypeId==1).Select(w=>w.Amount).ToList();
            var exist = _context.Wallet.Where(w => w.UserId == userId && w.TypeId == 2).Select(w => w.Amount).ToList();

            return (enter.Sum() + exist.Sum());

        }

        public EditProfileViewModel GetDataForEditProfileUser(string userName)
        {
            return _context.User.Where(u => u.UserName == userName).Select(u => new EditProfileViewModel()
            {
                AvatarName = u.UserAvatar,
                Email = u.Email,
                UserName = u.UserName,

            }).Single();
        }

        public MainPanelViewModel GetInformationForUserPanel(string userName)
        {
            var user=GetUserByUserName(userName);
            var info = _context.User.Where(w => w.UserName == userName).Select(c => new MainPanelViewModel
            {
                user = new InformationForUserPanelViewModel
                {
                    Email = c.Email,
                    AvatarName = c.UserAvatar,
                    RegisterDate = c.RegisterDate,
                    UserName = c.UserName,
                    Wallet=GetBallanceUserWallet(userName),
                }
            }).SingleOrDefault();
            return info;
        }

        public User GetUserByUserName(string userName)
        {
            return _context.User.SingleOrDefault(u=>u.UserName==userName);
        }

        public int GetUserIdByUserName(string userName)
        {
            return _context.User.Single(u=>u.UserName==userName).UserId;
        }

        public List<MainPanelViewModel> GetWalletForUser(string userName)
        {
            var wallet = _context.Wallet.Where(w => w.IsPay == true).Select(c=>new  MainPanelViewModel
            {
                wallet=new WalletViewModel
                {
                    Amount=c.Amount,
                    DateTime=c.CreateDate,
                    Description=c.Description,
                    Type=c.TypeId,
                }
            });
            return wallet.ToList();
        }

        public bool IsExistEmail(string email)
        {
            var fixedEmail = FixedText.FixedEmail(email);
            return _context.User.Any(x => x.Email == fixedEmail);
        }

        public bool IsExistUserName(string userName)
        {
            return _context.User.Any(x => x.UserName == userName);
        }

        public User Login(LoginViewModel model)
        {
            var hashPassword = PasswordHelper.EncodePasswordMd5(model.Password);
           return _context.User.SingleOrDefault(u=>u.UserName==model.UserName && u.Password==hashPassword);
        }

        public int Register(RegisterViewModel model)
        {
            var user = new User
            {
                ActiveCode =NameGenerator.GenerateUniqCode(),
                Email=model.Email,
                IsActive=true,
                IsDelete=false,
                RegisterDate=DateTime.Now,
                Password= PasswordHelper.EncodePasswordMd5(model.Password),
                UserAvatar="Default.jpg",
                UserName=model.UserName,
            };
            _context.User.Add(user);
            _context.SaveChanges();
            return user.UserId;

        }

        public UserForAdminViewModel GetUsers(int pageId = 1, string filterEmail = "", string filterUserName = "")
        {
            IQueryable<User> result = _context.User;

            if (!string.IsNullOrEmpty(filterEmail))
            {
                result = result.Where(u => u.Email.Contains(filterEmail));
            }

            if (!string.IsNullOrEmpty(filterUserName))
            {
                result = result.Where(u => u.UserName.Contains(filterUserName));
            }

            // Show Item In Page
            int take = 20;
            int skip = (pageId - 1) * take;


            UserForAdminViewModel list = new UserForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = result.Count() / take;
            list.Users = result.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();

            return list;
        }
        public int AddUser(User user)
        {
            _context.User.Add(user);
            _context.SaveChanges();
            return user.UserId;
        }
        public int AddUserFromAdmin(CreateUserViewModel user)
        {

            User addUser = new User();
            addUser.Password = PasswordHelper.EncodePasswordMd5(user.Password);
            addUser.ActiveCode = NameGenerator.GenerateUniqCode();
            addUser.Email = user.Email;
            addUser.IsActive = true;
            addUser.RegisterDate = DateTime.Now;
            addUser.UserName = user.UserName;

            #region Save Avatar

            if (user.UserAvatar != null)
            {
                string imagePath = "";
                addUser.UserAvatar = NameGenerator.GenerateUniqCode() + Path.GetExtension(user.UserAvatar.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", addUser.UserAvatar);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    user.UserAvatar.CopyTo(stream);
                }
            }

            #endregion

            return AddUser(addUser);
        }

        public EditUserViewModel GetUserForShowInEditMode(int userId)
        {
            return _context.User.Where(u => u.UserId == userId)
                .Select(u => new EditUserViewModel()
                {
                    UserId = u.UserId,
                    AvatarName = u.UserAvatar,
                    Email = u.Email,
                    UserName = u.UserName,
                    UserRoles = u.UserRoles.Select(r => r.RoleId).ToList()
                }).Single();
        }

        public void EditUserFromAdmin(EditUserViewModel editUser)
        {
            User user = GetUserById(editUser.UserId);
            user.Email = editUser.Email;
            if (!string.IsNullOrEmpty(editUser.Password))
            {
                user.Password = PasswordHelper.EncodePasswordMd5(editUser.Password);
            }

            if (editUser.UserAvatar != null)
            {
                //Delete old Image
                if (editUser.AvatarName != "Defult.jpg")
                {
                    string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", editUser.AvatarName);
                    if (File.Exists(deletePath))
                    {
                        File.Delete(deletePath);
                    }
                }

                //Save New Image
                user.UserAvatar = NameGenerator.GenerateUniqCode() + Path.GetExtension(editUser.UserAvatar.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar", user.UserAvatar);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    editUser.UserAvatar.CopyTo(stream);
                }
            }

            _context.User.Update(user);
            _context.SaveChanges();
        }

        public User GetUserById(int userId)
        {
            return _context.User.Find(userId);
        }

        public MainPanelViewModel GetInformationForUserPanel(int id)
        {
            var user = GetUserById(id);
            var info = _context.User.Where(w => w.UserId == id).Select(c => new MainPanelViewModel
            {
                user = new InformationForUserPanelViewModel
                {
                    Email = c.Email,
                    AvatarName = c.UserAvatar,
                    RegisterDate = c.RegisterDate,
                    UserName = c.UserName,
                    Wallet = GetBallanceUserWallet(user.UserName),
                }
            }).SingleOrDefault();
            return info;
        }

       

        public void UpdateUser(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }

        public UserForAdminViewModel GetDeleteUsers(int pageId = 1, string filterEmail = "", string filterUserName = "")
        {
            IQueryable<User> result = _context.User.IgnoreQueryFilters().Where(u => u.IsDelete);

            if (!string.IsNullOrEmpty(filterEmail))
            {
                result = result.Where(u => u.Email.Contains(filterEmail));
            }

            if (!string.IsNullOrEmpty(filterUserName))
            {
                result = result.Where(u => u.UserName.Contains(filterUserName));
            }

            // Show Item In Page
            int take = 20;
            int skip = (pageId - 1) * take;


            UserForAdminViewModel list = new UserForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = result.Count() / take;
            list.Users = result.OrderBy(u => u.RegisterDate).Skip(skip).Take(take).ToList();

            return list;
        }

        public void ReturnUser(int userId)
        {
            User user = GetDeletedUserById(userId);
            user.IsDelete = false;
            UpdateUser(user);
        }

        public User GetDeletedUserById(int id)
        {
            return _context.User.Where(c=>c.UserId==id).IgnoreQueryFilters().SingleOrDefault();
        }
    }
}
