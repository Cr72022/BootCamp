using BootCamp.Core.DTOs;
using BootCamp.DataLayer.Entities.User;
using BootCamp.DataLayer.Entities.Wallet;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootCamp.Core.Services.Interfaces
{
    public interface IAccountService
    {
        void UpdateUser(User user);
        void DeleteUser(int userId);
        void ReturnUser(int userId);
        User GetDeletedUserById(int id);
        bool IsExistEmail(string email);
        bool IsExistUserName(string userName);
        int Register(RegisterViewModel model);
        User Login(LoginViewModel model);
        User GetUserByUserName(string userName);
        int GetUserIdByUserName(string userName);
        MainPanelViewModel GetInformationForUserPanel(string userName);
        MainPanelViewModel GetInformationForUserPanel(int id);
        EditProfileViewModel GetDataForEditProfileUser(string userName);
        void EditProfile(string userName,EditProfileViewModel profile);
        void EditPassword(string userName, EditProfileViewModel profile);
        bool ComparePassword(string userName, string password);

        #region Wallet
        int GetBallanceUserWallet(string userName);
        List<MainPanelViewModel> GetWalletForUser(string userName);
        int ChargeWallet(string userName, int amount, string description, bool isPay = true);
        int AddWallet(Wallet wallet);
        int AddUser(User user);
        User GetUserById(int userId);
        #endregion
        #region Admin Panel

        UserForAdminViewModel GetUsers(int pageId = 1, string filterEmail = "", string filterUserName = "");
        UserForAdminViewModel GetDeleteUsers(int pageId = 1, string filterEmail = "", string filterUserName = "");
        int AddUserFromAdmin(CreateUserViewModel user);
        EditUserViewModel GetUserForShowInEditMode(int userId);
        void EditUserFromAdmin(EditUserViewModel editUser);

        #endregion


    }
}
