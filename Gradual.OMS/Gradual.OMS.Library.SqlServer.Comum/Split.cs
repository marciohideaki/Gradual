using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    [SqlFunction]
    public static SqlString Split(SqlString texto)
    {
        // Put your code here
        return new SqlString("Hello");
    }
};

