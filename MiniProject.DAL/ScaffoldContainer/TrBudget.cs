using System;
using System.Collections.Generic;

namespace MiniProject.DAL.ScaffoldContainer;

public partial class TrBudget
{
    public int Id { get; set; }

    public Guid? Guid { get; set; }

    public string JudulTransaksi { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? Nominal { get; set; }

    public decimal? OpenBalance { get; set; }

    public decimal? Addition { get; set; }

    public decimal? Deduction { get; set; }

    public int? IdKodepos { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual MKodepos? IdKodeposNavigation { get; set; }
}
