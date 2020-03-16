using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Text LevelText;
    public Slider Healthbar;
    public Button HitButton;

    public Animator AnimatorComponent;

    public delegate void EnemyHandler(int curLevel);
    public event EnemyHandler OnEnemyDeath;

    private int _health;
    private int _level;

    void Start()
    {
        Respawn(1);
    }

    public void OnHit()
    {
        AnimatorComponent.SetTrigger("Attacked");
        _health -= GameManager.HitStrength;

        if (_health <= 0)
        {
            AnimatorComponent.SetTrigger("Dead");
            OnEnemyDeath(_level);
            Respawn(++_level);
        }

        Healthbar.value = _health;
    }

    public void Respawn(int curLevel)
    {
        _level = curLevel;
        LevelText.text = $"Уровень {_level.ToString()}";
        _health = _level * 20;
        Healthbar.value = Healthbar.maxValue = _health;
    }
}
