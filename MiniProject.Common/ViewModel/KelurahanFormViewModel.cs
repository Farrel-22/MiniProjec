using System;
using System.ComponentModel.DataAnnotations;

namespace MiniProject.Common.ViewModel;

public class KelurahanFormViewModel
{
    public int? Id { get; set; }
    public Guid? Guid { get; set; }

    [Required(ErrorMessage = "Nama kelurahan wajib diisi.")]
    [StringLength(150, ErrorMessage = "Nama kelurahan maksimal 150 karakter.")]
    public string Nama { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kecamatan wajib dipilih.")]
    [Range(1, int.MaxValue, ErrorMessage = "Pilihan kecamatan tidak valid.")]
    public int IdKecamatan { get; set; }
}
