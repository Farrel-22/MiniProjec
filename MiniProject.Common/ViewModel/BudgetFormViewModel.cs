using System;
using System.ComponentModel.DataAnnotations;

namespace MiniProject.Common.ViewModel;

public class BudgetFormViewModel
{
    public int? Id { get; set; }
    public Guid? Guid { get; set; }

    [Required(ErrorMessage = "Judul transaksi wajib diisi.")]
    [StringLength(100, ErrorMessage = "Judul transaksi maksimal 100 karakter.")]
    public string JudulTransaksi { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Range(0, (double)decimal.MaxValue, ErrorMessage = "Open balance tidak boleh negatif.")]
    public decimal? OpenBalance { get; set; } = 0;

    [Range(0, (double)decimal.MaxValue, ErrorMessage = "Addition tidak boleh negatif.")]
    public decimal? Addition { get; set; } = 0;

    [Range(0, (double)decimal.MaxValue, ErrorMessage = "Deduction tidak boleh negatif.")]
    public decimal? Deduction { get; set; } = 0;

    public decimal? Nominal { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Pilihan kode pos tidak valid.")]
    public int? IdKodepos { get; set; }

    public decimal EndingBalance => (OpenBalance ?? 0) + (Addition ?? 0) - (Deduction ?? 0);
}
