﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Belt.Migrations
{
    public partial class again2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BidPrice",
                table: "bids",
                nullable: false,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "BidPrice",
                table: "bids",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
