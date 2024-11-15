extends CharacterBody2D

class_name Koopa


@export var SPEED: int = 40
@export var IN_SHELL_SPEED: int = 150
var direction: int = -1
var in_shell: bool = false
var moving: bool = false
var timer: float = 0.0

@onready var raycast_right = $RayCastRight
@onready var raycast_left = $RayCastLeft
@onready var sprite = $AnimatedSprite as AnimatedSprite2D
@onready var animation_player = $AnimationPlayer
@onready var squished = $Area2D
@onready var death_sound: AudioStreamPlayer2D = $DeathSound


func kick_shell(angle: float):
	if (in_shell):
		if (angle > 90):
			direction = 1
		else:
			direction = -1
		death_sound.play()
		moving = true



func enter_shell():
	if (in_shell and timer <= 0.0 and !moving):
		kick_shell(91)
	elif (in_shell and timer <= 0.0 and moving):
		moving = false
		velocity.x = 0
	else:
		timer = 0.25
		in_shell = true
		sprite.play("in_shell")
		animation_player.play("shell")
		moving = false
		velocity.x = 0

func handle_non_squish_death():
	sprite.rotate(deg_to_rad(180))
	animation_player.play("death_by_shell")
	var spawn_tween = get_tree().create_tween()
	spawn_tween.tween_property(self, "position", position + Vector2(0, -16), 0.2)
	spawn_tween.tween_property(self, "position", position + Vector2(0, 32), 0.2)

func _ready() -> void:
	squished.connect("squished", enter_shell)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if (timer > 0.0):
		timer -= delta


	if raycast_right.is_colliding():
		var collider = raycast_right.get_collider()

		if (in_shell && !moving && collider is Player):
			moving = true
			kick_shell(0)
		if (in_shell):
			if (collider is Pipe):
				direction = -1
				sprite.flip_h = false
			elif (moving && collider is Goomba):
				(collider as Goomba).handle_non_squish_death()
		else:
			direction = -1
			sprite.flip_h = false
	if raycast_left.is_colliding():
		var collider = raycast_left.get_collider()

		if (in_shell && !moving && collider is Player):
			moving = true
			kick_shell(91)

		if (in_shell):
			if (collider is Pipe):
				direction = 1
				sprite.flip_h = true
			elif (moving && collider is Goomba):
				(collider as Goomba).handle_non_squish_death()
		else:
			direction = 1
			sprite.flip_h = true

	# Handle that you jumped on the enemy's head
	# if raycast_top.is_colliding():
	# 	var collided_with = raycast_top.get_collider()

	# 	if collided_with:
	# 		if collided_with.has_method("bounce"):
	# 			collided_with.bounce()
	# 			isSquished = true


func _physics_process(delta: float) -> void:
	if in_shell:
		if moving:
			velocity.x = direction * IN_SHELL_SPEED
	else:
		velocity.x = direction * SPEED

	if not is_on_floor():
		velocity += get_gravity() * delta

	move_and_slide()

func die_from_hit() -> void:
	handle_non_squish_death()
