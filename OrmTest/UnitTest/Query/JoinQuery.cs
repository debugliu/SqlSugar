﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using System.Linq.Expressions;
using OrmTest.Models;
namespace OrmTest.UnitTest
{
    public class JoinQuery : ExpTestBase
    {
        private JoinQuery() { }
        public JoinQuery(int eachCount)
        {
            this.Count = eachCount;
        }
        internal void Init()
        {
            base.Begin();
            for (int i = 0; i < base.Count; i++)
            {
                Q1();
                Q2();
            }
            base.End("Method Test");
        }

        public void Q1()
        {
            using (var db = GetInstance())
            {
                var join1 = db.Queryable<Student, School>((st, sc) => new object[] {
                          JoinType.Left,st.SchoolId==sc.Id
                }).Where(st => st.Id > 0).Select<Student>("*").ToSql();
                base.Check(@"SELECT * FROM [Student] st Left JOIN School sc ON ( [st].[SchoolId] = [sc].[Id] )   WHERE ( [st].[Id] > @Id0 ) ",
                    new List<SugarParameter>() {
                        new SugarParameter("@Id0",0)
                    }, join1.Key, join1.Value, "join 1 Error");
            }
        }
        public void Q2()
        {
            using (var db = GetInstance())
            {
                var join2 = db.Queryable<Student, School>((st, sc) => new object[] {
                          JoinType.Left,st.SchoolId==sc.Id
                }).Where(st=>st.Id>2).Select<Student>("*").ToSql();
                base.Check(@"SELECT * FROM [Student] st Left JOIN School sc ON ( [st].[SchoolId] = [sc].[Id] )   WHERE ( [st].[Id] > @Id0 ) ",
    new List<SugarParameter>() {
                        new SugarParameter("@Id0",2)
    }, join2.Key, join2.Value, "join 1 Error");
            }
        }


        public SqlSugarClient GetInstance()
        {
            SqlSugarClient db = new SqlSugarClient(new SystemTablesConfig() { ConnectionString = Config.ConnectionString, DbType = DbType.SqlServer });
            db.Database.IsEnableLogEvent = true;
            db.Database.LogEventStarting = (sql, pars) =>
            {
                Console.WriteLine(sql+" "+pars);
            };
            return db;
        }
    }
}
