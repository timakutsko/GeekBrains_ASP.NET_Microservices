using FluentMigrator;
using MetricsManager.MySQLSettings;
using System;

namespace MetricsManager.DAL.Migrations

{ 
    [Migration(1)] 
    public class FirstMigration : Migration 
    {
        /// <summary>
        /// Объект с именами и настройками базы данных
        /// </summary>
        private readonly IMySqlSettings _mySql;

        public FirstMigration(IMySqlSettings mySqlSettings)
        {
            _mySql = mySqlSettings;
        }

        public override void Up() 
        {
            // Подключаю таблицу с агентами
            Create.Table($"{_mySql.AgentsTable}").
                    WithColumn($"{_mySql[Columns.AgentId]}").AsInt64().PrimaryKey().Identity().
                    WithColumn($"{_mySql[Columns.AgentUri]}").AsInt64();
            
            // Подключаю таблицы с данными по агентам
            foreach (Tables table in Enum.GetValues(typeof(Tables)))
            {
                Create.Table($"{_mySql[table]}").
                    WithColumn($"{_mySql[Columns.Id]}").AsInt64().PrimaryKey().Identity().
                    WithColumn($"{_mySql[Columns.AgentId]}").AsInt32().
                    WithColumn($"{_mySql[Columns.Value]}").AsInt32().
                    WithColumn($"{_mySql[Columns.Time]}").AsInt64(); 
            }
        } 
        public override void Down() 
        {
            // Удаляю таблицы с данными
            foreach (Tables table in Enum.GetValues(typeof(Tables)))
            {
                Delete.Table($"{_mySql[table]}");
            }

            // Удаляю таблицы с данными по агентам
            Delete.Table(_mySql.AgentsTable);

        } 
    } 
}
