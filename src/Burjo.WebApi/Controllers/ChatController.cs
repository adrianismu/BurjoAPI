using Burjo.Core.DTOs;
using Burjo.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Burjo.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    /// <summary>
    /// Send message to chatbot and get response
    /// </summary>
    [HttpPost("send")]
    public async Task<ActionResult<ChatResponseDto>> SendMessage([FromBody] ChatMessageDto chatMessageDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var response = await _chatService.GetResponseAsync(chatMessageDto.Message, userId);

            return Ok(new
            {
                message = "Chat response generated successfully",
                userMessage = chatMessageDto.Message,
                botResponse = response
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                message = "An error occurred while processing chat message", 
                error = ex.Message 
            });
        }
    }

    /// <summary>
    /// Get chat welcome message and quick replies
    /// </summary>
    [HttpGet("welcome")]
    public ActionResult<ChatResponseDto> GetWelcomeMessage()
    {
        try
        {
            var welcomeResponse = new ChatResponseDto
            {
                Response = "Selamat datang di Bugar Rogo Jiwo Chat! 🤖✨\n\n" +
                          "Saya adalah asisten virtual Anda untuk kesehatan dan kebugaran. " +
                          "Saya siap membantu dengan:\n\n" +
                          "• 📊 Mood tracking dan analisis\n" +
                          "• 🏃‍♂️ Rekomendasi olahraga personal\n" +
                          "• 🏥 Tips kesehatan dan wellness\n" +
                          "• 💪 Motivasi dan semangat hidup sehat\n" +
                          "• ❓ Bantuan penggunaan aplikasi\n\n" +
                          "Silakan ketik pesan atau pilih salah satu opsi di bawah untuk memulai!",
                Timestamp = DateTime.UtcNow,
                QuickReplies = new List<string> 
                { 
                    "Halo, apa kabar?",
                    "Cek mood hari ini", 
                    "Rekomendasi olahraga", 
                    "Tips hidup sehat",
                    "Motivasi dong!"
                }
            };

            return Ok(new
            {
                message = "Welcome message retrieved successfully",
                data = welcomeResponse
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                message = "An error occurred while getting welcome message", 
                error = ex.Message 
            });
        }
    }

    /// <summary>
    /// Get chat help and available commands
    /// </summary>
    [HttpGet("help")]
    public ActionResult<ChatResponseDto> GetChatHelp()
    {
        try
        {
            var helpResponse = new ChatResponseDto
            {
                Response = "🆘 **Panduan Chat Bugar Rogo Jiwo** 🆘\n\n" +
                          "**Kata kunci yang saya pahami:**\n\n" +
                          "🎭 **Mood & Perasaan:**\n" +
                          "• \"mood\", \"perasaan\", \"suasana hati\"\n" +
                          "• \"cek mood\", \"catat mood\"\n\n" +
                          "🏃‍♂️ **Olahraga & Fitness:**\n" +
                          "• \"olahraga\", \"latihan\", \"rekomendasi\"\n" +
                          "• \"jadwal\", \"fitness\", \"senam\"\n\n" +
                          "🏥 **Kesehatan:**\n" +
                          "• \"kesehatan\", \"risiko\", \"penilaian\"\n" +
                          "• \"sehat\", \"medis\", \"dokter\"\n\n" +
                          "💪 **Motivasi:**\n" +
                          "• \"motivasi\", \"semangat\", \"malas\"\n" +
                          "• \"lelah\", \"putus asa\"\n\n" +
                          "❓ **Bantuan:**\n" +
                          "• \"bantuan\", \"help\", \"cara\"\n" +
                          "• \"bagaimana\", \"panduan\"\n\n" +
                          "💡 **Tips:** Gunakan bahasa natural, saya akan berusaha memahami maksud Anda!",
                Timestamp = DateTime.UtcNow,
                QuickReplies = new List<string> 
                { 
                    "Coba tanya mood",
                    "Tanya tentang olahraga", 
                    "Minta motivasi",
                    "Kembali ke menu utama"
                }
            };

            return Ok(new
            {
                message = "Chat help retrieved successfully",
                data = helpResponse
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                message = "An error occurred while getting chat help", 
                error = ex.Message 
            });
        }
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user ID in token");
        }
        return userId;
    }
}
