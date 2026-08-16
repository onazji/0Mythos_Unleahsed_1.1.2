using UnityEngine;

/// <summary>
/// Neutral archetype shell for a Sanctum side branch.
///
/// MazeBranchController owns WHEN the branch changes phase.
/// BranchArchetypePrototype describes WHAT kind of branch this is
/// and provides convenient entry/completion hooks.
///
/// Dungeon Architect should eventually spawn the prefab.
/// It should not need to understand the branch's internal mechanics.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(MazeBranchController))]
public sealed class BranchArchetypePrototype : MonoBehaviour
{
    public enum Archetype
    {
        Undefined,

        // Pressure through confrontation.
        Fury,

        // Attention, perception, concealment, discovery.
        Veil,

        // Movement, rhythm, traversal, momentum.
        Flow,

        // Rule recognition and manipulation.
        Puzzle,

        // Exploration, observation, environmental discovery.
        Discovery
    }

    [Header("Archetype Identity")]
    [SerializeField]
    private Archetype archetype = Archetype.Undefined;

    [TextArea]
    [SerializeField]
    private string designerIntent;

    [Header("Controller")]
    [SerializeField]
    private MazeBranchController controller;

    [Header("Debug")]
    [SerializeField]
    private bool logBranchActions = true;

    public Archetype Type => archetype;

    public MazeBranchController Controller => controller;

    public bool IsResolved =>
        controller != null && controller.HasResolved;

    private void Reset()
    {
        FindController();
    }

    private void Awake()
    {
        FindController();

        if (controller == null)
        {
            Debug.LogError(
                $"[BranchArchetypePrototype] '{name}' has no MazeBranchController.",
                this
            );
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (controller == null)
        {
            FindController();
        }
    }
#endif

    private void FindController()
    {
        controller = GetComponent<MazeBranchController>();
    }

    /// <summary>
    /// Called when the player discovers or enters the branch.
    ///
    /// Recognition should communicate:
    /// "What kind of experience is this?"
    /// </summary>
    public void Recognize()
    {
        if (controller == null)
        {
            return;
        }

        Log("Recognition started.");

        controller.BeginRecognition();
    }

    /// <summary>
    /// Called when the player commits to the branch activity.
    ///
    /// Examples:
    /// Fury  -> arena locks / enemies activate
    /// Veil  -> perception challenge begins
    /// Flow  -> traversal sequence begins
    /// Puzzle -> puzzle becomes interactive
    /// </summary>
    public void Participate()
    {
        if (controller == null)
        {
            return;
        }

        Log("Participation started.");

        controller.BeginParticipation();
    }

    /// <summary>
    /// Called by the branch-specific victory condition.
    ///
    /// Examples:
    /// Fury  -> final enemy dies
    /// Flow  -> destination reached
    /// Puzzle -> solution accepted
    /// Veil  -> hidden truth discovered
    /// </summary>
    public void Resolve()
    {
        if (controller == null)
        {
            return;
        }

        Log("Resolution started.");

        controller.ResolveBranch();
    }

    /// <summary>
    /// Returns the archetype to Dormant.
    /// Primarily useful during development and for repeatable content.
    /// </summary>
    public void ResetArchetype()
    {
        if (controller == null)
        {
            return;
        }

        Log("Branch reset.");

        controller.ResetBranch();
    }

    private void Log(string message)
    {
        if (!logBranchActions)
        {
            return;
        }

        Debug.Log(
            $"[BranchArchetypePrototype:{archetype}] {message}",
            this
        );
    }
}