extends Area2D


@onready var timer = $Timer
@onready var audio_stream_player_2d: AudioStreamPlayer = $GameOverMusic
# Better to make this a singleton, not this way. But I will fix in a real game instead of a clone
@onready var music: AudioStreamPlayer = %Music


func _on_body_entered(body:Node2D) -> void:
	if (body is Player):
		print("You died!")
		music.stop()
		audio_stream_player_2d.play()
	else:
		queue_free()


func _on_game_over_music_finished() -> void:
	timer.start()


func _on_timer_timeout() -> void:
	get_tree().reload_current_scene()
