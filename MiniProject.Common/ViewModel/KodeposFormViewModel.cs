using System;
using System.ComponentModel.DataAnnotations;

namespace MiniProject.Common.ViewModel;

public class KodeposFormViewModel
{
    public int? Id { get; set; }
    public Guid? Guid { get; set; }

    [Required(ErrorMessage = "Kode pos wajib diisi.")]
    [StringLength(10, ErrorMessage = "Kode pos maksimal 10 karakter.")]
    public string Kodepos { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kelurahan wajib dipilih.")]
    [Range(1, int.MaxValue, ErrorMessage = "Pilihan kelurahan tidak valid.")]
    public int IdKelurahan { get; set; }
}
