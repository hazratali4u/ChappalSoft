using System;
using System.Data;
using System.Data.SqlClient;
using ChappalDAL;

namespace ChappalBLL
{
    public class PartyController
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public DataTable GetParty(int TypeID)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@TypeID",TypeID)
            };
            return dbHelper.ExecuteDataTable("uspGetParty", parameters);
        }

        public int InsertUpdateParty(int PartyID, string Name, string Address,string ContactNo,int PartyType, int OpeningBalance, int UpdatedBy, int TypeID,int DBServer)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@PartyID", PartyID),
                new SqlParameter("@Name",Name),
                new SqlParameter("@Address", Address),
                new SqlParameter("@ContactNo", ContactNo),
                new SqlParameter("@PartyType",PartyType),
                new SqlParameter("@OpeningBalance",OpeningBalance),
                new SqlParameter("@UpdatedBy", UpdatedBy),
                new SqlParameter("@TypeID", TypeID),
                new SqlParameter("@DBServer",DBServer)
            };
            return dbHelper.ExecuteNonQuery("uspInsertUpdateParty", parameters);
        }

        public int InsertCustomer(string Name, string Address, string ContactNo, int UpdatedBy,int DBServer)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@PartyID", 0),
                new SqlParameter("@Name",Name),
                new SqlParameter("@Address", Address),
                new SqlParameter("@ContactNo", ContactNo),
                new SqlParameter("@PartyType",1),
                new SqlParameter("@OpeningBalance",0),
                new SqlParameter("@UpdatedBy", UpdatedBy),
                new SqlParameter("@TypeID", 1),
                new SqlParameter("@DBServer",DBServer)
            };
            return Convert.ToInt32(dbHelper.ExecuteScalar("uspInsertUpdateParty", parameters));
        }
    }
}