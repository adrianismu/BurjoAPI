namespace Burjo.Core.Enums;

public enum Gender
{
    Male = 1,
    Female = 2,
    Other = 3
}

public enum ActivityLevel
{
    Sedentary = 1,        // Sedikit atau tidak ada olahraga
    LightlyActive = 2,    // Olahraga ringan 1-3 hari/minggu
    ModeratelyActive = 3, // Olahraga sedang 3-5 hari/minggu
    VeryActive = 4,       // Olahraga berat 6-7 hari/minggu
    ExtremelyActive = 5   // Olahraga sangat berat atau pekerjaan fisik
}

public enum FitnessGoal
{
    WeightLoss = 1,      // Menurunkan berat badan
    WeightGain = 2,      // Menaikkan berat badan
    MaintainWeight = 3,  // Mempertahankan berat badan
    BuildMuscle = 4,     // Membangun otot
    ImproveEndurance = 5, // Meningkatkan daya tahan
    GeneralFitness = 6   // Kebugaran umum
}

public enum WorkoutType
{
    Cardio = 1,
    Strength = 2,
    Flexibility = 3,
    Balance = 4,
    Sports = 5,
    Mixed = 6
}

public enum ExerciseDifficulty
{
    Beginner = 1,
    Intermediate = 2,
    Advanced = 3,
    Expert = 4
}

public enum RiskCategory
{
    Aman = 1,               // Aman untuk berolahraga
    PengawasanRingan = 2,   // Perlu pengawasan ringan
    SupervisiMedis = 3      // Perlu supervisi medis
}

public enum MoodType
{
    SangatBaik = 1,         // Sangat baik/excellent
    Baik = 2,               // Baik/good
    Sedang = 3,             // Sedang/neutral
    Buruk = 4               // Buruk/bad
}
