using System;
using System.Collections.Generic;

namespace MiniProject.DAL.ScaffoldContainer;

public partial class MKodepos
{
    public int Id { get; set; }

    public Guid? Guid { get; set; }

    public string Kodepos { get; set; } = null!;

    public int IdKelurahan { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual MKelurahan IdKelurahanNavigation { get; set; } = null!;

    public virtual ICollection<TrBudget> TrBudgets { get; set; } = new List<TrBudget>();
}
