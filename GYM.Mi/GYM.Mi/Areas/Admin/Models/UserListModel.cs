using GYM.Mi.Domain;
using System.Data;

namespace GYM.Mi.Areas.Admin.Models
{
    public class UserListModel:DataTables
    {
        public UserSearchModel SearchItem { get; set; } = new UserSearchModel();
    }
}
