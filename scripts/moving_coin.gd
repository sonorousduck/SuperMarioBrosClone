extends Node2D

class_name MovingCoin



func _ready() -> void:
	var spawn_tween = get_tree().create_tween()
	spawn_tween.tween_property(self, "position", position + Vector2(0, -64), 0.3)
	spawn_tween.tween_property(self, "position", position + Vector2(0, -32), 0.3)
	spawn_tween.tween_callback(func(): queue_free())


