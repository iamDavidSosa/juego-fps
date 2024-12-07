using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public TextMeshProUGUI EnemigosText;

    private int enemigosVivos = 0;

    public void EnemyAlive()
    {
        enemigosVivos++;
        UpdateEnemiesUI();
    }

    public void EnemyDied()
    {
        enemigosVivos--;
        UpdateEnemiesUI();
    }

    private void UpdateEnemiesUI()
    {
        EnemigosText.text = $"Enemigos: {enemigosVivos}";
    }
}
