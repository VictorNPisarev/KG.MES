using System.Text.Json.Serialization;

namespace KG.MES.Shared.Models.Dto
{
	public class OrderCommentDto
	{
		[JsonPropertyName("id")]
		public Guid Id { get; set; }

		[JsonPropertyName("content")]
		public string? Content { get; set; } = string.Empty;

		[JsonPropertyName("created_at")]
		public DateTime? CreatedAt { get; set; }

		[JsonPropertyName("updated_at")]
		public DateTime? UpdatedAt { get; set; }

		[JsonPropertyName("user_name")]
		public string? UserName { get; set; }
	}

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
}