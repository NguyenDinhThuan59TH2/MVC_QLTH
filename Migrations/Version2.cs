using System.Data.Entity.Migrations;
using System.Security.Permissions;

public partial class InitialCreate : DbMigration
{
    public override void Up()
    {
        CreateTable(
            "dbo.TaiKhoanKhachHang",
            c => new
            {
                MaKH = c.String(nullable: false),
                TaiKhoan = c.String(nullable: false),
                MatKhau = c.String(nullable: false),
            })
            .PrimaryKey(t => t.TaiKhoan)
            .ForeignKey("dbo.KhachHang", t => t.MaKH, cascadeDelete: true)
            .Index(t => t.MaKH);
    }

    public override void Down()
    {
        DropForeignKey("dbo.TaiKhoanKhachHang", "MaKH", "dbo.KhachHang");
        DropIndex("dbo.TaiKhoanKhachHang", new[] { "MaKH" });
        DropTable("dbo.TaiKhoanKhachHang");
    }
}