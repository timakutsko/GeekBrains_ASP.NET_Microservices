using FluentMigrator;
using MetricAgent.MySQLSettings;
using System;

namespace MetricsAgent.DAL.Migrations

{ 
    [Migration(1)] 
    public class FirstMigration : Migration 
    {
        private readonly IMySqlSettings _mySql;

        public FirstMigration(IMySqlSettings mySqlSettings)
        {
            _mySql = mySqlSettings;
        }

        /// <summary>
        /// Подключить таблицы для записи
        /// </summary>
        public override void Up() 
        {
            foreach (Tables table in Enum.GetValues(typeof(Tables)))
            {
                Create.Table($"{_mySql[table]}").
                    WithColumn($"{_mySql[Columns.Id]}").AsInt64().PrimaryKey().Identity().
                    WithColumn($"{_mySql[Columns.Value]}").AsInt32().
                    WithColumn($"{_mySql[Columns.Time]}").AsInt64(); 
            }
        } 
        
        /// <summary>
        /// Удалить таблицы с данными
        /// </summary>
        public override void Down() 
        {
            foreach (Tables table in Enum.GetValues(typeof(Tables)))
            {
                Delete.Table($"{_mySql[table]}");
            }
        } 
    } 
}
