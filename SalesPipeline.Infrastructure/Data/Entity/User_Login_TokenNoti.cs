using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class User_Login_TokenNoti
{
    public int Id { get; set; }

    public DateTime CreateDate { get; set; }

    public int UserId { get; set; }

    public string? DeviceId { get; set; }

    public string? tokenNoti { get; set; }
}
