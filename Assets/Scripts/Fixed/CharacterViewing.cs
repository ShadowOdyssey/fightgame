using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterViewing : MonoBehaviour
{
    public Texture2D background; // Background image
    public Texture2D[] characterImages; // Array to hold character images
    public Texture2D buttonLeftImage; // Texture for the left button
    public Texture2D buttonRightImage; // Texture for the right button
    public Texture2D buttonExitImage; // Texture for the exit button

    public AudioClip[] heroVoiceClips; // Array of voice clips for each hero
    private AudioSource audioSource; // AudioSource component

    private int currentIndex = 0; // Index to keep track of the current character
    private string[] characterNames = { "Selena", "Aria", "Orion", "Marcus", "Gabriela", "Nun", "Bryan","Oliver" }; // Character names

    // Character stats and stories
    private string[] characterStories =
    {
        "Selena is a fearless fighter with striking white hair and a signature black combat outfit. Known for her extraordinary agility and stealth, she earned her reputation by entering an underground tournament. The arena is a ruthless proving ground, where only the strongest survive. Selena's speed and precision made her an instant crowd favorite. During the final match, she faced a mysterious adversary known only as The Phantom, whose speed rivaled her own. The battle was fierce, pushing Selena to her limits. Her decisive spinning kick became legendary, marking her as a champion among fighters. But victory came with a price. The mysterious figure hinted at something far more dangerous than any tournament—an ancient evil awakening in the deepest corners of the universe. Now, Selena must face this new challenge, leaving the arena behind to join forces with others capable of standing against the coming storm.",

        "In the enchanted forests of her realm, Aria serves as the Guardian of Nature, wielding powers drawn from the elements around her. Her armor, adorned with the intricate designs of the forest, is both a symbol of her duty and a shield that protects her from harm. Aria’s strength lies not only in her physical abilities but in her deep connection with the natural world. When the harmony of her world is disturbed by a strange surge of dark energy, Aria realizes the source is not from her own realm. To protect her forest, she must leave it, joining Selena on a quest to confront the growing evil. Together, they seek out the final piece of their team—a warrior who possesses strength beyond imagination.",

        "Orion hails from a realm beyond time and space, a place where warriors are trained to protect the fabric of reality itself. His black and orange suit, adorned with geometric patterns, enhances his abilities, allowing him to travel through dimensions. With immense physical power, Orion’s strength is legendary. Orion’s mission is to find and defeat worthy opponents across the dimensions to grow even stronger. But his journey takes a sudden turn when he discovers an anomaly—a rift between dimensions caused by the rising dark force. Realizing the scale of the threat, Orion chooses to ally with Selena and Aria, knowing that only together can they prevent the destruction of all worlds.",

        "Marcus is a tactical mastermind, able to command legions with precision and skill. With a cold demeanor and a sharp mind, Marcus rose through the ranks of his kingdom's military to become the youngest general in history. His ability to strategize under pressure makes him a formidable leader. Though respected, Marcus carries the burden of past mistakes—decisions made that cost him dearly. Now, with a looming threat endangering not only his world but countless others, Marcus knows that his tactical genius may be the key to their survival. He joins Selena, Aria, and Orion to plan the ultimate battle against the forces of darkness.",

        "Gabriela, the Shadowblade, is a master of stealth and subterfuge. Raised in the secretive order of the Night Watchers, Gabriela learned the arts of assassination and evasion. As a protector of the innocent, she walks the fine line between light and darkness. When the balance of power in her world shifts due to the encroaching darkness, Gabriela seeks out allies to confront this threat. Her cunning and agility make her a vital addition to the team.",

        "Nun, the Celestial Guardian, is a being of light, wielding divine power to protect the realms from malevolent forces. Clad in radiant armor that reflects the stars, Nun has the ability to heal allies and shield them from harm. His mission is to restore balance and harmony, fighting against the forces of darkness that threaten to consume the universe. His journey brings him to join forces with Selena, Aria, Orion, and Gabriela. Together, they stand against the rising evil, combining their powers to protect all that is good.",

        "Bryan, a battle-hardened warrior from a distant war-torn land, carries the weight of countless battles on his shoulders. His rugged armor is scarred from encounters with brutal enemies, and his eyes tell the story of a survivor. Bryan's indomitable will makes him a fearsome opponent. His unmatched strength and resilience make him the cornerstone of any defensive line. Despite his rough exterior, Bryan fights for peace, hoping to one day see an end to the endless wars that have defined his life.",

       " Oliver was forced to adapt when the village was destroyed and his mentor killed. Now, he guards the *Abandoned Forge*, the last remaining forge in the region. His ability to fight is rooted in his resourcefulness—using tools and objects around him as weapons. Oliver’s experience in craftsmanship has turned him into a skilled and unpredictable fighter.",
    };

    private int[] durabilityStatsStats = { 80, 70, 95, 85, 75, 90, 100, 90 }; // Strength stats
    private int[] offenseStats = { 90, 60, 70, 80, 95, 60, 50, 85 };   // Agility stats
    private int[] controlStats = { 60, 85, 80, 75, 70, 90, 95, 90 }; // Durability stats
    private int[] difficultyStats = { 70, 75, 80, 75, 80, 90, 100,95 }; // Durability stats

    public void Start()
    {
        // Get or add an AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false;
        // No selectSound playback here
    }

    void OnGUI()
    {
        DrawBackground();
        DrawCharacterPanel();
        DrawHeroDetails();
        DrawStatsPanel(); // New panel for stats bars
        DrawNavigationButtons();
        DrawExitButton();
    }

    void DrawBackground()
    {
        if (background != null)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background, ScaleMode.StretchToFill);
        }
    }

    void DrawCharacterPanel()
    {
        if (characterImages.Length > 0)
        {
            float imageWidth = Screen.width * 0.5f;
            float imageHeight = Screen.height * 0.8f;
            float imageX = Screen.width * 0.06f; 
            float imageY = Screen.height * 0.1f;

            GUI.DrawTexture(new Rect(imageX, imageY, imageWidth, imageHeight), characterImages[currentIndex], ScaleMode.ScaleToFit);
        }
    }

    void DrawHeroDetails()
{
    float detailsWidth = Screen.width * 0.5f;
    float detailsHeight = Screen.height * 0.7f;
    float detailsX = Screen.width * 0.5f;
    float detailsY = Screen.height * 0.1f;

    GUI.Box(new Rect(detailsX, detailsY, detailsWidth, detailsHeight), "");

    GUIStyle nameStyle = new GUIStyle(GUI.skin.label)
    {
        fontSize = 50,
        fontStyle = FontStyle.Bold,
        alignment = TextAnchor.MiddleCenter,
        normal = { textColor = Color.yellow }
    };

    // Validate characterNames array
    if (currentIndex >= 0 && currentIndex < characterNames.Length)
    {
        GUI.Label(new Rect(detailsX + 20, detailsY + 20, detailsWidth - 40, 40), characterNames[currentIndex], nameStyle);
    }

    GUIStyle storyStyle = new GUIStyle(GUI.skin.label)
    {
        fontSize = 40,
        wordWrap = true,
        alignment = TextAnchor.UpperLeft,
        normal = { textColor = Color.white }
    };

    float storyHeight = 600;

    // Validate characterStories array
    if (currentIndex >= 0 && currentIndex < characterStories.Length)
    {
        GUI.Label(new Rect(detailsX + 20, detailsY + 70, detailsWidth - 40, storyHeight), characterStories[currentIndex], storyStyle);
    }
}

    void DrawStatsPanel()
    {
        float statsWidth = Screen.width * 0.2f;
        float statsHeight = Screen.height * 0.3f;
        float statsX = Screen.width * 0.01f;
        float statsY = Screen.height * 0.1f;

        GUI.Box(new Rect(statsX, statsY, statsWidth, statsHeight), "");

        float padding = 40;
        GUIStyle statsStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 25,
            normal = { textColor = Color.white } 
        };

        float barWidth = statsWidth - padding * 2;
        float barHeight = 20;

        DrawStatBar(new Rect(statsX + padding, statsY + 60, barWidth, barHeight), durabilityStatsStats[currentIndex], "Durability");
        DrawStatBar(new Rect(statsX + padding, statsY + 130, barWidth, barHeight), offenseStats[currentIndex], "Offense");
        DrawStatBar(new Rect(statsX + padding, statsY + 200, barWidth, barHeight), controlStats[currentIndex], "Control");
         DrawStatBar(new Rect(statsX + padding, statsY + 270, barWidth, barHeight), difficultyStats[currentIndex], "Difficulty");
    }

    void DrawStatBar(Rect position, int statValue, string statName)
    {
        GUI.Label(new Rect(position.x, position.y - 50, position.width, 40), statName, new GUIStyle(GUI.skin.label) { fontSize = 30, normal = { textColor = Color.green } });

        float statPercentage = statValue / 100f; // Assuming max stat value is 100
        GUI.Box(position, ""); // Box for background
        GUI.DrawTexture(new Rect(position.x, position.y, position.width * statPercentage, position.height), Texture2D.whiteTexture); // Green fill for stats
    }

    void DrawNavigationButtons()
    {
        if (GUI.Button(new Rect(Screen.width * 0.2f, Screen.height * 0.9f, 100, 50), buttonLeftImage))
        {
            NavigateToHero(-1);
        }

        if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.9f, 100, 50), buttonRightImage))
        {
            NavigateToHero(1);
        }
    }

    void DrawExitButton()
    {
        if (GUI.Button(new Rect(Screen.width - 120, 20, 100, 50), buttonExitImage))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    void NavigateToHero(int direction)
    {
        currentIndex += direction;
        if (currentIndex < 0) currentIndex = characterImages.Length - 1;
        if (currentIndex >= characterImages.Length) currentIndex = 0;

        if (heroVoiceClips.Length > 0 && audioSource != null)
        {
            audioSource.clip = heroVoiceClips[currentIndex];
            audioSource.Play();
        }
    }
}
