
using KG.MES.Shared.Models.Dto;

namespace KG.MES.Shared.ViewModels;
public class OrderCommentViewModel : OrderCommentDto
{
	/// <summary>
	/// Локальное состояние: новый (ещё не сохранён) или существующий.
	/// </summary>
	public bool IsNew { get; set; }
	/// <summary>
	/// Локальное состояние: редактируется ли сейчас.
	/// </summary>
	public bool IsEditing { get; set; }
}