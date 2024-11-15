using NUnit.Framework;
using UnityEngine;

public class TestSuite
{
    private GameManager gameManager;
    private PlayerController playerController;

    [SetUp]
    public void SetUp()
    {
        // Creamos el objeto del GameManager
        GameObject gameManagerObj = new GameObject();
        gameManager = gameManagerObj.AddComponent<GameManager>();

        // Creamos el objeto del Player y agregamos los componentes esenciales para el test
        GameObject playerObj = new GameObject();
        playerController = playerObj.AddComponent<PlayerController>();
        playerController.gameObject.AddComponent<Rigidbody2D>();  // Necesario para la fisica de movimiento
        playerController.gameObject.AddComponent<Collider2D>();   // Necesario para detectar colisiones

        // Asignamos playerController al GameManager
        gameManager.playerController = playerController;

        // Inicializamos los valores de GameManager para este test
        gameManager.isGameOver = false; // Aseguramos que no este en Game Over
    }

    // Test para la funcion Death
    [Test]
    public void TestMuerte()
    {
        gameManager.Death();  // Llamamos al metodo Death de GameManager

        Assert.IsTrue(gameManager.isGameOver);  // Verificamos que el estado de isGameOver haya cambiado a true
        Assert.IsFalse(gameManager.playerController.gameObject.activeSelf);  // Verificamos que el GameObject del jugador esté desactivado
    }

    // Test para la funcion FindTotalPickups
    [Test]
    public void TestTotalDeRecolectables()
    {
        // Creamos pickups de tipo moneda
        GameObject coin1 = new GameObject();
        var pickup1 = coin1.AddComponent<pickup>();
        pickup1.pt = pickup.pickupType.coin;

        GameObject coin2 = new GameObject();
        var pickup2 = coin2.AddComponent<pickup>();
        pickup2.pt = pickup.pickupType.coin;

        // Creamos un pickup de otro tipo para verificar que no sea contado
        GameObject gem = new GameObject();
        var pickup3 = gem.AddComponent<pickup>();
        pickup3.pt = pickup.pickupType.gem;

        gameManager.FindTotalPickups();  // Llamamos al metodo para contar pickups
        Assert.AreEqual(2, gameManager.totalCoins, "El total de monedas recogidas deberia ser 2");
    }

    // Test para verificar el respawn del jugador despues de la muerte
    [Test]
    public void TestRespawnAfterDeath()
    {
        Vector3 initialPosition = gameManager.playerController.transform.position;

        gameManager.Death();  // Llamamos el metodo de la muerte

        // Simulamos que el Coroutine de muerte se ha completado
        gameManager.StartCoroutine(gameManager.DeathCoroutine()); 

        // Verificamos que el jugador ha vuelto a su posicion inicial
        Assert.AreEqual(initialPosition, gameManager.playerController.transform.position, "El jugador deberia regresar a su posición original después de morir y resucitar");
    }
}
