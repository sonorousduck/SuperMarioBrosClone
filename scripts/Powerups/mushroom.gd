extends Area2D

class_name Mushroom

@export var horizontal_speed = 20
@export var max_vertical_speed = 175
@export var vertical_velocity_gain = 0.1
@export var bounce_vertical_velocity_gain = .75

@export var bounce_speed = 75

@onready var shape_cast = $ShapeCast2D

var allow_horizontal_movement = false
var vertical_speed = 0
var bouncing: bool = false
var bouncing_timer: float = 0.30


func _ready() -> void:
	var spawn_tween = get_tree().create_tween()
	spawn_tween.tween_property(self, "position", position + Vector2(0, -16), 0.4)
	spawn_tween.tween_callback(func (): 
		allow_horizontal_movement = true )


func _process(delta: float) -> void:
	if allow_horizontal_movement:
		position.x += delta * horizontal_speed
	
	if bouncing:
		bouncing_timer -= delta
		vertical_speed = lerpf(vertical_speed, max_vertical_speed, bounce_vertical_velocity_gain)
		position.y -= delta * vertical_speed

		if bouncing_timer <= 0:
			bouncing_timer = .30
			bouncing = false
			vertical_speed = 0



	if !shape_cast.is_colliding() && allow_horizontal_movement && !bouncing:
		vertical_speed = lerpf(vertical_speed, max_vertical_speed, vertical_velocity_gain)
		position.y += delta * vertical_speed
	else:
		vertical_speed = 0


func bump() -> void:
	bouncing = true

