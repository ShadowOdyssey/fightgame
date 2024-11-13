using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterViewing : MonoBehaviour
{
    public Texture2D background; // Background image
    public Texture2D[] characterImages; // Array to hold character images
    public Texture2D buttonLeftImage; // Texture for the left button
    public Texture2D buttonRightImage; // Texture for the right button
    public Texture2D buttonExitImage; // Texture for the exit button

    public AudioClip selectSound; // Sound clip to play when selecting a character
    private AudioSource audioSource; // AudioSource component

    private int currentIndex = 0; // Index to keep track of the current character
    private string[] characterNames = { "Selena", "Aria", "Orion", "Marcus", "Gabriela", "Nun", "Bryan" }; // Character names

    // Character stats and stories
    private string[] characterStories = 
    {
        "Selena is a fearless fighter with striking white hair and a signature black combat outfit. Known for her extraordinary agility and stealth, she earned her reputation by entering an underground tournament. The arena is a ruthless proving ground, where only the strongest survive. Selena's speed and precision made her an instant crowd favorite. During the final match, she faced a mysterious adversary known only as The Phantom, whose speed rivaled her own. The battle was fierce, pushing Selena to her limits. Her decisive spinning kick became legendary, marking her as a champion among fighters. But victory came with a price. The mysterious figure hinted at something far more dangerous than any tournament—an ancient evil awakening in the deepest corners of the universe. Now, Selena must face this new challenge, leaving the arena behind to join forces with others capable of standing against the coming storm.",
        
        "In the enchanted forests of her realm, Aria serves as the Guardian of Nature, wielding powers drawn from the elements around her. Her armor, adorned with the intricate designs of the forest, is both a symbol of her duty and a shield that protects her from harm. Aria’s strength lies not only in her physical abilities but in her deep connection with the natural world. When the harmony of her world is disturbed by a strange surge of dark energy, Aria realizes the source is not from her own realm. To protect her forest, she must leave it, joining Selena on a quest to confront the growing evil. Together, they seek out the final piece of their team—a warrior who possesses strength beyond imagination.",
        
        "Orion hails from a realm beyond time and space, a place where warriors are trained to protect the fabric of reality itself. His black and orange suit, adorned with geometric patterns, enhances his abilities, allowing him to travel through dimensions. With immense physical power, Orion’s strength is legendary. Orion’s mission is to find and defeat worthy opponents across the dimensions to grow even stronger. But his journey takes a sudden turn when he discovers an anomaly—a rift between dimensions caused by the rising dark force. Realizing the scale of the threat, Orion chooses to ally with Selena and Aria, knowing that only together can they prevent the destruction of all worlds.",
        
        "Marcus is a tactical mastermind, able to command legions with precision and skill. With a cold demeanor and a sharp mind, Marcus rose through the ranks of his kingdom's military to become the youngest general in history. His ability to strategize under pressure makes him a formidable leader. Though respected, Marcus carries the burden of past mistakes—decisions made that cost him dearly. Now, with a looming threat endangering not only his world but countless others, Marcus knows that his tactical genius may be the key to their survival. He joins Selena, Aria, and Orion to plan the ultimate battle against the forces of darkness.",
        
        "Gabriela, the Shadowblade, is a master of stealth and subterfuge. Raised in the secretive order of the Night Watchers, Gabriela learned the arts of assassination and evasion. As a protector of the innocent, she walks the fine line between light and darkness. When the balance of power in her world shifts due to the encroaching darkness, Gabriela seeks out allies to confront this threat. Her cunning and agility make her a vital addition to the team.",
        
        "Nun, the Celestial Guardian, is a being of light, wielding divine power to protect the realms from malevolent forces. Clad in radiant armor that reflects the stars, Nun has the ability to heal allies and shield them from harm. His mission is to restore balance and harmony, fighting against the forces of darkness that threaten to consume the universe. His journey brings him to join forces with Selena, Aria, Orion, and Gabriela. Together, they stand against the rising evil, combining their powers to protect all that is good.",
        
        "Bryan, a battle-hardened warrior from a distant war-torn land, carries the weight of countless battles on his shoulders. His rugged armor is scarred from encounters with brutal enemies, and his eyes tell the story of a survivor. Bryan's indomitable will makes him a fearsome opponent. His unmatched strength and resilience make him the cornerstone of any defensive line. Despite his rough exterior, Bryan fights for peace, hoping to one day see an end to the endless wars that have defined his life."
    };

    private int[] strengthStats = { 80, 70, 95, 85, 75, 90, 100 }; // Strength stats
    private int[] agilityStats = { 90, 60, 70, 80, 95, 60, 50 };   // Agility stats
    private int[] durabilityStats = { 60, 85, 80, 75, 70, 90, 95 }; // Durability stats

    private void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnGUI()
    {
        // Draw background image
        DrawBackground();

        // Draw the character selection panel
        DrawCharacterPanel();

        // Draw hero details panel on the side
        DrawHeroDetails();

        // Draw left and right buttons for navigation
        DrawNavigationButtons();

        // Draw exit button at the top-right corner
        DrawExitButton();
    }

    void DrawBackground()
    {
        // Draw background image if one is provided
        if (background != null)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background, ScaleMode.StretchToFill);
        }
    }

    void DrawCharacterPanel()
    {
        // Draw character image on the left side of the screen
        if (characterImages.Length > 0)
        {
            float imageWidth = Screen.width * 0.4f;
            float imageHeight = Screen.height * 0.8f;
            float imageX = Screen.width * 0.05f; // Position on the left
            float imageY = Screen.height * 0.1f;

            GUI.DrawTexture(new Rect(imageX, imageY, imageWidth, imageHeight), characterImages[currentIndex], ScaleMode.ScaleToFit);
        }
    }

    void DrawHeroDetails()
    {
        // Draw hero details (story and stats) on the right side of the screen
        float detailsWidth = Screen.width * 0.5f;
        float detailsHeight = Screen.height * 0.7f;
        float detailsX = Screen.width * 0.5f; // Position on the right
        float detailsY = Screen.height * 0.1f;

        GUI.Box(new Rect(detailsX, detailsY, detailsWidth, detailsHeight), ""); // Panel for hero details

        // Draw character name
        GUIStyle nameStyle = new GUIStyle(GUI.skin.label) 
        { 
            fontSize = 50, // Increased font size
            fontStyle = FontStyle.Bold, 
            alignment = TextAnchor.MiddleCenter, // Center text alignment
            normal = { textColor = Color.yellow } // Color for the name
        };
        GUI.Label(new Rect(detailsX + 20, detailsY + 20, detailsWidth - 40, 40), characterNames[currentIndex], nameStyle);

        // Story Text
        GUIStyle storyStyle = new GUIStyle(GUI.skin.label) 
        { 
            fontSize = 30, // Adjust font size for better readability
            wordWrap = true,
            alignment = TextAnchor.UpperLeft, // Align story to the left
            normal = { textColor = Color.white } // Color for the story
        };

        // Set a fixed height for the story area
        float storyHeight = 400; // Fixed height for story text
        GUI.Label(new Rect(detailsX + 20, detailsY + 70, detailsWidth - 40, storyHeight), characterStories[currentIndex], storyStyle);

        // Draw stats
        GUIStyle statsStyle = new GUIStyle(GUI.skin.label) 
        { 
            fontSize = 24, // Adjust font size for stats
            normal = { textColor = Color.green } // Color for the stats
        };
        
        GUI.Label(new Rect(detailsX + 20, detailsY + 480, detailsWidth - 40, 30), "Strength: " + strengthStats[currentIndex], statsStyle);
        GUI.Label(new Rect(detailsX + 20, detailsY + 510, detailsWidth - 40, 30), "Agility: " + agilityStats[currentIndex], statsStyle);
        GUI.Label(new Rect(detailsX + 20, detailsY + 540, detailsWidth - 40, 30), "Durability: " + durabilityStats[currentIndex], statsStyle);
    }

    void DrawNavigationButtons()
    {
        // Draw left button
        if (GUI.Button(new Rect(Screen.width * 0.1f, Screen.height * 0.9f, 100, 50), buttonLeftImage))
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = characterImages.Length - 1; // Loop back to last character
            audioSource.PlayOneShot(selectSound);
            Debug.Log("Current Index after left: " + currentIndex);
        }

        // Draw right button
        if (GUI.Button(new Rect(Screen.width * 0.3f, Screen.height * 0.9f, 100, 50), buttonRightImage))
        {
            currentIndex++;
            if (currentIndex >= characterImages.Length) currentIndex = 0; // Loop back to first character
            audioSource.PlayOneShot(selectSound);
            Debug.Log("Current Index after right: " + currentIndex);
        }
    }

    void DrawExitButton()
    {
        // Adjust the button's position if necessary for visibility
        if (GUI.Button(new Rect(Screen.width - 120, 20, 100, 50), buttonExitImage))
        {
            // Load the main menu scene
            SceneManager.LoadScene("MainMenu"); // Ensure "MainMenu" matches your scene name
            Debug.Log("Exit button pressed, loading Main Menu.");
        }
    }
}
