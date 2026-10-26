using System;
using System.Collections.Generic;

namespace MiniProject.DAL.ScaffoldContainer;

public partial class MKelurahan
{
    public int Id { get; set; }

    public Guid? Guid { get; set; }

    public string Nama { get; set; } = null!;

    public int IdKecamatan { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual MKecamatan IdKecamatanNavigation { get; set; } = null!;

    public virtual ICollection<MKodepos> MKodepos { get; set; } = new List<MKodepos>();
}
