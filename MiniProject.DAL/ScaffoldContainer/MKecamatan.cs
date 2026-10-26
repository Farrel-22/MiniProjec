using System;
using System.Collections.Generic;

namespace MiniProject.DAL.ScaffoldContainer;

public partial class MKecamatan
{
    public int Id { get; set; }

    public Guid? Guid { get; set; }

    public string Nama { get; set; } = null!;

    public int IdKota { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual MKota IdKotaNavigation { get; set; } = null!;

    public virtual ICollection<MKelurahan> MKelurahans { get; set; } = new List<MKelurahan>();
}
