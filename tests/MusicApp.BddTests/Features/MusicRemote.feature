Feature: Control de música con comandos
  Como usuario
  Quiero controlar reproducción con un control remoto
  Para ejecutar Play, Pause y Skip mediante comandos

  Scenario Outline: Ejecutar comando desde el control remoto
    Given un reproductor de música
    And un control remoto de música
    And el comando <command> configurado en el control remoto
    When presiono el botón del control remoto
    Then recibo el mensaje <message>

    Examples:
      | command | message                       |
      | Play    | Playing the song.             |
      | Pause   | Pausing the song.             |
      | Skip    | Skipping to the next song.    |