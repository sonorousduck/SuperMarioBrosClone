extends Area2D

signal squished



# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass


func _on_body_entered(body:Node2D) -> void:
	if (body.has_method("bounce")):
		body.bounce()
		# squished.emit()
		get_parent().isSquished = true
		squished.emit()
		# if (get_parent() is Goomba):
		# 	get_parent().isSquished = true
		# 	squished.emit()
		# elif (get_parent() is Koopa):
		# 	get_parent().enter_shell()
