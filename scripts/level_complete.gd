extends Area2D

@onready var slide_down_position: Node2D = $SlideDownPosition

func _on_body_entered(body):
	if body is Player:
		(body as Player).on_pole_hit(slide_down_position.position.y + global_position.y)
