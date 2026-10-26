using System;
using System.ComponentModel.DataAnnotations;

namespace MiniProject.Common.ViewModel;

public class KotaFormViewModel
{
    public int? Id { get; set; }
    public Guid? Guid { get; set; }

    [Required(ErrorMessage = "Nama kota wajib diisi.")]
    [StringLength(150, ErrorMessage = "Nama kota maksimal 150 karakter.")]
    public string Nama { get; set; } = string.Empty;
}
