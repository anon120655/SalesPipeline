using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class User_Login_Log
{
    public int Id { get; set; }

    public DateTime CreateDate { get; set; }

    public int UserId { get; set; }

    public string? FullName { get; set; }

    public string? IPAddress { get; set; }

    public string? DeviceId { get; set; }

    public string? DeviceVersion { get; set; }

    public string? SystemVersion { get; set; }

    public string? AppVersion { get; set; }

    public string? tokenNoti { get; set; }
}
