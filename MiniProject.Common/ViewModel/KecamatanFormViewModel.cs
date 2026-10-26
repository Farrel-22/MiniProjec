using System;
using System.ComponentModel.DataAnnotations;

namespace MiniProject.Common.ViewModel;

public class KecamatanFormViewModel
{
    public int? Id { get; set; }
    public Guid? Guid { get; set; }

    [Required(ErrorMessage = "Nama kecamatan wajib diisi.")]
    [StringLength(150, ErrorMessage = "Nama kecamatan maksimal 150 karakter.")]
    public string Nama { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kota wajib dipilih.")]
    [Range(1, int.MaxValue, ErrorMessage = "Pilihan kota tidak valid.")]
    public int IdKota { get; set; }
}
