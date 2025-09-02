using Burjo.Core.DTOs;
using Burjo.Core.Interfaces;
using Burjo.Core.Models;

namespace Burjo.Infrastructure.Services;

public class ChatService : IChatService
{
    private readonly IMoodService _moodService;
    private readonly IHealthService _healthService;
    private readonly IRecommendationService _recommendationService;
    private readonly IChatTimeValidationService _timeValidationService;
    private readonly ChatSettings _chatSettings;

    public ChatService(
        IMoodService moodService,
        IHealthService healthService,
        IRecommendationService recommendationService,
        IChatTimeValidationService timeValidationService,
        ChatSettings chatSettings)
    {
        _moodService = moodService;
        _healthService = healthService;
        _recommendationService = recommendationService;
        _timeValidationService = timeValidationService;
        _chatSettings = chatSettings;
    }

    public async Task<ChatResponseDto> GetResponseAsync(string userMessage, Guid userId)
    {
        var message = userMessage.ToLowerInvariant().Trim();
        var response = new ChatResponseDto
        {
            Timestamp = DateTime.UtcNow
        };

        try
        {
            // Check if message contains emergency keywords first
            var isEmergency = _timeValidationService.ContainsEmergencyKeywords(userMessage);
            
            // If emergency detected, prioritize emergency response regardless of time
            if (isEmergency)
            {
                response.IsEmergencyDetected = true;
                response.Response = _timeValidationService.GetEmergencyMessage();
                response.QuickReplies = new List<string> 
                { 
                    "Hubungi 119", 
                    "Cari IGD terdekat", 
                    "Tips pertolongan pertama"
                };
                return response;
            }

            // Check operating hours for non-emergency messages
            var isWithinHours = _timeValidationService.IsWithinOperatingHours();
            response.IsWithinOperatingHours = isWithinHours;
            
            if (!isWithinHours)
            {
                response.Response = _timeValidationService.GetOutOfHoursMessage();
                response.OperatingHoursInfo = this.GetOperatingHoursInfo().Message;
                response.QuickReplies = new List<string> 
                { 
                    "Cek jam operasional", 
                    "Tips kesehatan umum", 
                    "Kontak darurat"
                };
                return response;
            }

            // Normal chat processing within operating hours
            // Greeting patterns
            if (IsGreeting(message))
            {
                response.Response = "Halo! Selamat datang di Bugar Rogo Jiwo! 👋 Saya di sini untuk membantu perjalanan kesehatan dan kebugaran Anda. Apa yang bisa saya bantu hari ini?";
                response.QuickReplies = new List<string> 
                { 
                    "Cek mood hari ini", 
                    "Lihat rekomendasi olahraga", 
                    "Bantuan kesehatan",
                    "Tips motivasi"
                };
                return response;
            }

            // Mood-related queries
            if (IsMoodQuery(message))
            {
                return await HandleMoodQuery(message, userId);
            }

            // Exercise/fitness related queries
            if (IsExerciseQuery(message))
            {
                return await HandleExerciseQuery(message, userId);
            }

            // Health-related queries
            if (IsHealthQuery(message))
            {
                return await HandleHealthQuery(message, userId);
            }

            // Motivation queries
            if (IsMotivationQuery(message))
            {
                return HandleMotivationQuery();
            }

            // Help queries
            if (IsHelpQuery(message))
            {
                return HandleHelpQuery();
            }

            // Default response for unrecognized queries
            response.Response = "Maaf, saya belum memahami pertanyaan Anda. Berikut beberapa hal yang bisa saya bantu:";
            response.QuickReplies = new List<string> 
            { 
                "Cek mood", 
                "Rekomendasi olahraga", 
                "Tips kesehatan",
                "Motivasi",
                "Bantuan"
            };
            return response;
        }
        catch
        {
            response.Response = "Maaf, terjadi kesalahan. Silakan coba lagi nanti.";
            return response;
        }
    }

    private static bool IsGreeting(string message)
    {
        var greetings = new[] { "halo", "hai", "hi", "hello", "selamat", "pagi", "siang", "sore", "malam" };
        return greetings.Any(greeting => message.Contains(greeting));
    }

    private static bool IsMoodQuery(string message)
    {
        var moodKeywords = new[] { "mood", "perasaan", "suasana hati", "bahagia", "sedih", "senang", "kesal" };
        return moodKeywords.Any(keyword => message.Contains(keyword));
    }

    private static bool IsExerciseQuery(string message)
    {
        var exerciseKeywords = new[] { "olahraga", "latihan", "senam", "fitness", "gym", "lari", "jalan", "yoga", "rekomendasi" };
        return exerciseKeywords.Any(keyword => message.Contains(keyword));
    }

    private static bool IsHealthQuery(string message)
    {
        var healthKeywords = new[] { "kesehatan", "sehat", "sakit", "penyakit", "dokter", "medis", "risiko" };
        return healthKeywords.Any(keyword => message.Contains(keyword));
    }

    private static bool IsMotivationQuery(string message)
    {
        var motivationKeywords = new[] { "motivasi", "semangat", "malas", "lelah", "putus asa", "menyerah" };
        return motivationKeywords.Any(keyword => message.Contains(keyword));
    }

    private static bool IsHelpQuery(string message)
    {
        var helpKeywords = new[] { "bantuan", "help", "bagaimana", "cara", "tolong", "panduan" };
        return helpKeywords.Any(keyword => message.Contains(keyword));
    }

    private async Task<ChatResponseDto> HandleMoodQuery(string message, Guid userId)
    {
        var response = new ChatResponseDto { Timestamp = DateTime.UtcNow };

        try
        {
            var latestMood = await _moodService.GetLatestMoodAsync(userId);
            var hasLoggedToday = await _moodService.HasLoggedMoodTodayAsync(userId);

            if (message.Contains("cek") || message.Contains("lihat"))
            {
                if (latestMood != null)
                {
                    response.Response = $"Mood terakhir Anda adalah '{latestMood.MoodText}' pada {latestMood.LoggedAt:dd/MM/yyyy HH:mm}. ";
                    
                    if (!hasLoggedToday)
                    {
                        response.Response += "Anda belum mencatat mood hari ini. Yuk catat mood Anda!";
                        response.SuggestedActions = "Silakan buka halaman mood untuk mencatat perasaan Anda hari ini.";
                    }
                    else
                    {
                        response.Response += "Terima kasih sudah mencatat mood hari ini! 😊";
                    }
                }
                else
                {
                    response.Response = "Anda belum pernah mencatat mood. Yuk mulai mencatat mood harian untuk tracking kesehatan mental Anda!";
                    response.SuggestedActions = "Buka halaman mood untuk mulai mencatat perasaan Anda.";
                }
            }
            else
            {
                response.Response = "Mencatat mood harian sangat penting untuk kesehatan mental! Mood tracking membantu Anda memahami pola emosi dan faktor-faktor yang mempengaruhinya.";
                response.SuggestedActions = "Jangan lupa catat mood Anda setiap hari ya!";
            }

            response.QuickReplies = new List<string> { "Catat mood sekarang", "Lihat riwayat mood", "Tips mood baik" };
            return response;
        }
        catch
        {
            response.Response = "Maaf, terjadi kesalahan saat mengecek mood Anda.";
            return response;
        }
    }

    private Task<ChatResponseDto> HandleExerciseQuery(string message, Guid userId)
    {
        var response = new ChatResponseDto { Timestamp = DateTime.UtcNow };

        try
        {
            if (message.Contains("rekomendasi"))
            {
                response.Response = "Saya akan memberikan rekomendasi olahraga yang sesuai dengan kondisi kesehatan Anda. Rekomendasi ini berdasarkan penilaian risiko dari data kesehatan yang sudah Anda isi.";
                response.SuggestedActions = "Buka halaman rekomendasi olahraga untuk melihat latihan yang cocok untuk Anda.";
                response.QuickReplies = new List<string> { "Lihat rekomendasi", "Buat jadwal olahraga", "Tips olahraga" };
            }
            else if (message.Contains("jadwal"))
            {
                response.Response = "Membuat jadwal olahraga rutin adalah kunci sukses dalam hidup sehat! Saya sarankan untuk berolahraga 3-5 kali seminggu dengan durasi 30-60 menit per sesi.";
                response.SuggestedActions = "Yuk buat jadwal olahraga mingguan yang sesuai dengan aktivitas Anda.";
                response.QuickReplies = new List<string> { "Buat jadwal", "Lihat jadwal saya", "Tips konsisten olahraga" };
            }
            else
            {
                response.Response = "Olahraga teratur memberikan banyak manfaat: meningkatkan mood, memperkuat jantung, menurunkan stress, dan meningkatkan kualitas tidur. Yang penting adalah konsistensi, bukan intensitas!";
                response.QuickReplies = new List<string> { "Rekomendasi olahraga", "Buat jadwal", "Motivasi olahraga" };
            }

            return Task.FromResult(response);
        }
        catch
        {
            response.Response = "Maaf, terjadi kesalahan saat memproses pertanyaan olahraga Anda.";
            return Task.FromResult(response);
        }
    }

    private Task<ChatResponseDto> HandleHealthQuery(string message, Guid userId)
    {
        var response = new ChatResponseDto { Timestamp = DateTime.UtcNow };

        try
        {
            if (message.Contains("risiko") || message.Contains("penilaian"))
            {
                response.Response = "Penilaian risiko kesehatan membantu menentukan jenis olahraga yang aman untuk Anda. Berdasarkan data kesehatan seperti riwayat penyakit dan keluhan fisik, sistem akan memberikan kategori: Aman, Pengawasan Ringan, atau Supervisi Medis.";
                response.SuggestedActions = "Pastikan data kesehatan Anda sudah lengkap untuk penilaian yang akurat.";
                response.QuickReplies = new List<string> { "Cek penilaian risiko", "Update data kesehatan", "Tips hidup sehat" };
            }
            else
            {
                response.Response = "Kesehatan adalah investasi terbaik! Tips hidup sehat: makan bergizi seimbang, olahraga teratur, tidur cukup 7-8 jam, kelola stress, dan rutin check-up kesehatan.";
                response.QuickReplies = new List<string> { "Penilaian risiko", "Tips nutrisi", "Kelola stress" };
            }

            return Task.FromResult(response);
        }
        catch
        {
            response.Response = "Maaf, terjadi kesalahan saat memproses pertanyaan kesehatan Anda.";
            return Task.FromResult(response);
        }
    }

    private static ChatResponseDto HandleMotivationQuery()
    {
        var motivationalQuotes = new[]
        {
            "\"Kesehatan adalah kekayaan yang sesungguhnya, bukan emas atau perak.\" - Mahatma Gandhi",
            "\"Tubuh yang sehat adalah tempat jiwa yang bahagia.\" - Juvenal",
            "\"Perubahan dimulai dari langkah pertama, tidak peduli seberapa kecil.\"",
            "\"Konsistensi kecil mengalahkan usaha besar yang tidak konsisten.\"",
            "\"Investasi terbaik adalah investasi pada kesehatan diri sendiri.\"",
            "\"Setiap hari adalah kesempatan baru untuk menjadi versi terbaik dari diri Anda.\"",
            "\"Jangan tunggu sempurna, mulai dari sekarang dengan apa yang Anda punya.\""
        };

        var randomQuote = motivationalQuotes[Random.Shared.Next(motivationalQuotes.Length)];

        return new ChatResponseDto
        {
            Response = $"Semangat! 💪 {randomQuote}\n\nIngat, perjalanan hidup sehat bukan tentang sempurna, tapi tentang konsistensi. Setiap langkah kecil yang Anda ambil hari ini adalah investasi untuk kesehatan masa depan!",
            Timestamp = DateTime.UtcNow,
            QuickReplies = new List<string> { "Tips tetap semangat", "Mulai olahraga", "Catat mood baik" }
        };
    }

    private static ChatResponseDto HandleHelpQuery()
    {
        return new ChatResponseDto
        {
            Response = "Saya siap membantu Anda! 🤖 Berikut yang bisa saya bantu:\n\n" +
                      "📱 **Fitur Utama:**\n" +
                      "• Mood tracking - Catat dan pantau mood harian\n" +
                      "• Rekomendasi olahraga - Saran latihan sesuai kondisi kesehatan\n" +
                      "• Jadwal olahraga - Buat dan kelola jadwal mingguan\n" +
                      "• Penilaian risiko - Analisis kesehatan untuk olahraga aman\n\n" +
                      "💬 **Chat dengan saya tentang:**\n" +
                      "• Tips hidup sehat\n" +
                      "• Motivasi dan semangat\n" +
                      "• Panduan penggunaan aplikasi\n" +
                      "• Saran olahraga dan kesehatan",
            Timestamp = DateTime.UtcNow,
            QuickReplies = new List<string> { "Mulai mood tracking", "Lihat rekomendasi", "Buat jadwal", "Tips sehat" }
        };
    }

    public ChatOperatingHoursDto GetOperatingHoursInfo()
    {
        var isWithinHours = _timeValidationService.IsWithinOperatingHours();
        var currentTime = DateTime.Now.ToString("HH:mm");
        var operatingHours = $"{_chatSettings.OperatingHours.StartTime} - {_chatSettings.OperatingHours.EndTime} WIB";
        
        if (isWithinHours)
        {
            return new ChatOperatingHoursDto
            {
                IsOpen = true,
                CurrentTime = currentTime,
                OperatingHours = operatingHours,
                Message = $"✅ Layanan chat sedang AKTIF (jam {operatingHours})\n" +
                         $"⏰ Waktu sekarang: {currentTime} WIB\n\n" +
                         $"Saya siap membantu Anda dengan:\n" +
                         $"• Konsultasi kesehatan dan kebugaran\n" +
                         $"• Rekomendasi olahraga personal\n" +
                         $"• Mood tracking dan motivasi\n" +
                         $"• Tips hidup sehat"
            };
        }
        else
        {
            var timeUntilNext = _timeValidationService.GetTimeUntilNextOperatingHour();
            var nextOpenTime = DateTime.Now.Add(timeUntilNext).ToString("dd/MM/yyyy HH:mm");
            
            return new ChatOperatingHoursDto
            {
                IsOpen = false,
                CurrentTime = currentTime,
                OperatingHours = operatingHours,
                NextOpenTime = nextOpenTime,
                Message = $"🔒 Layanan chat sedang TUTUP\n" +
                         $"⏰ Jam operasional: {operatingHours}\n" +
                         $"⏰ Waktu sekarang: {currentTime} WIB\n" +
                         $"🕐 Buka kembali: {nextOpenTime} WIB\n\n" +
                         $"Untuk keadaan darurat medis, hubungi 119 atau IGD terdekat."
            };
        }
    }
}
