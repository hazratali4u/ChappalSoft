using System.Data;
using System.Data.SqlClient;
using ChappalDAL;

namespace ChappalBLL
{
    public class UserController
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public DataTable GetUser(string LoginID, string Password,int TypeID,int DBServer)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@LoginID", LoginID),                
                new SqlParameter("@Password", Password),
                new SqlParameter("@TypeID",TypeID),
                new SqlParameter("@DBServer",DBServer)
            };
            return dbHelper.ExecuteDataTable("uspGetUser", parameters);
        }

        public int InsertUpdateUser(int UserID,string EmployeeName, string LoginID, string Password, int RoleID, int CreatedBy, int TypeID,int DBServer)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@UserID", UserID),
                new SqlParameter("@EmployeeName",EmployeeName),
                new SqlParameter("@LoginID", LoginID),
                new SqlParameter("@Password", Password),
                new SqlParameter("@RoleID", RoleID),
                new SqlParameter("@CreatedBy", CreatedBy),
                new SqlParameter("@TypeID", TypeID),
                new SqlParameter("@DBServer",DBServer)
            };
            return dbHelper.ExecuteNonQuery("uspInsertUpdateUser", parameters);
        }
    }
}