extends Node2D


# Called when the node enters the scene tree for the first time.
# func _ready() -> void:
# 	pass # Replace with function body.

const SPEED: int = 60
var direction: int = 1

@onready var raycast_right = $RayCastRight
@onready var raycast_left = $RayCastLeft
@onready var sprite = $Sprite2D

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if raycast_right.is_colliding():
		direction = -1
		sprite.flip_h = true
	if raycast_left.is_colliding():
		direction = 1
		sprite.flip_h = false


func _physics_process(delta: float) -> void:
	position.x += direction * SPEED * delta 
