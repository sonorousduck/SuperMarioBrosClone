extends CharacterBody2D

class_name Koopa


@export var SPEED: int = 40
@export var IN_SHELL_SPEED: int = 150
var direction: int = -1
var in_shell: bool = false
var moving: bool = false


@onready var raycast_right = $RayCastRight
@onready var raycast_left = $RayCastLeft
@onready var sprite = $AnimatedSprite as AnimatedSprite2D
@onready var animation_player = $AnimationPlayer


func kick_shell(angle: float):
	if (in_shell):
		if (angle > 90):
			direction = 1
		else:
			direction = -1
		
		moving = true



func enter_shell():
	if in_shell:
		moving = false
	else:
		in_shell = true
		animation_player.play("shell")


func _ready() -> void:
	var squished = get_node("Area2D")
	squished.connect("squished", enter_shell)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if raycast_right.is_colliding():

		direction = -1
		sprite.flip_h = true
	if raycast_left.is_colliding():
		direction = 1
		sprite.flip_h = false

	# Handle that you jumped on the enemy's head
	# if raycast_top.is_colliding():
	# 	var collided_with = raycast_top.get_collider()

	# 	if collided_with:
	# 		if collided_with.has_method("bounce"):
	# 			collided_with.bounce()
	# 			isSquished = true
		

func _physics_process(delta: float) -> void:
	if in_shell:
		velocity.x = direction * IN_SHELL_SPEED
	else:
		velocity.x = direction * SPEED

	if not is_on_floor():
		velocity += get_gravity() * delta

	move_and_slide()

func die_from_hit() -> void:
	queue_free()
