using System;
using System.Collections.Generic;

namespace MiniProject.DAL.ScaffoldContainer;

public partial class MKota
{
    public int Id { get; set; }

    public Guid? Guid { get; set; }

    public string Nama { get; set; } = null!;

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<MKecamatan> MKecamatans { get; set; } = new List<MKecamatan>();
}
