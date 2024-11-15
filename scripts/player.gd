extends CharacterBody2D

class_name Player

const SPEED = 200.0
const GRAVITY_MODIFIER = 2
const JUMP_VELOCITY = -350.0 
const BOUNCE_VELOCITY = -250.0
const RUN_SPEED_DAMPING = 0.5
var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")


const SMALL_MARIO_COLLISION_SHAPE = preload("res://Resources/CollisionShapes/SmallMario.tres")
const BIG_MARIO_COLLISION_SHAPE = preload("res://Resources/CollisionShapes/BigMario.tres")

@onready var animation_player = $AnimationPlayer
@onready var area_2d = $Area2D
@onready var collisionShape2D = $CollisionShape2D
@onready var animated_sprite = $AnimatedSprite
@onready var music: AudioStreamPlayer2D = $"../Music"
@onready var invincibility_timer: Timer = $InvincibilityTimer
@onready var game_manager: GameManager = %GameManager

var point_bonus = 0

var invincible = false


enum PlayerMode {
	SMALL,
	BIG,
	FIRE
}

var player_mode = PlayerMode.SMALL

func handle_movement_collision(collision: KinematicCollision2D):
	if (collision.get_collider() is Block):
		var collision_angle = rad_to_deg(collision.get_angle())
		if roundf(collision_angle) == 180:
			(collision.get_collider() as Block).bump(player_mode)
		
	if (collision.get_collider() is Brick):
		var collision_angle = rad_to_deg(collision.get_angle())
		if roundf(collision_angle) == 180:
			(collision.get_collider() as Brick).bump(player_mode)
		
	if (collision.get_collider() is Pipe):
		var pipe = collision.get_collider() as Pipe
		var collision_angle = rad_to_deg(collision.get_angle())
		if (roundf(collision_angle) == 0 && Input.is_action_just_pressed("Down")) && pipe.enterable:
			print("TODO: Go into pipe, if possible")

# This function is used when the player kills an enemy
func bounce():
	velocity.y = BOUNCE_VELOCITY	
	


func _physics_process(delta: float) -> void:
	print(player_mode)
	# Add the gravity.
	if not is_on_floor():
		velocity.y += gravity * delta
	else:
		point_bonus = 0
	

	if Input.is_action_just_pressed("esc"):
		get_tree().quit()


	# Handle jump.
	if Input.is_action_just_pressed("jump") and is_on_floor():
		velocity.y = JUMP_VELOCITY

		if (player_mode == PlayerMode.SMALL):
			animation_player.play("small_jump")
		else:
			animation_player.play("big_jump")

	if Input.is_action_just_released("jump") and velocity.y < 0:
		velocity.y *= 0.5;

	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	var direction := Input.get_axis("move_left", "move_right")
	if direction:
		velocity.x = lerpf(velocity.x, SPEED * direction, RUN_SPEED_DAMPING * delta)
	else: 
		velocity.x = move_toward(velocity.x, 0, SPEED * delta)


	
	animated_sprite.trigger_animation(velocity, direction, player_mode)


	var collision = get_last_slide_collision()
	if collision != null:
		handle_movement_collision(collision)



	move_and_slide()


func handle_mushroom_collision():
	if player_mode == PlayerMode.SMALL:
		set_physics_process(false)
		animated_sprite.play("small_to_big")
		animation_player.play("small_to_big")
		
		#set_collision_shapes(true)

func handle_enemy_collision():
	if invincible:
		return
	# TODO: Check health
	# Reduce down if not big or flower
	match (player_mode):
		PlayerMode.SMALL:
			animation_player.play("death")
		PlayerMode.BIG:
			set_physics_process(false)
			animated_sprite.play("big_to_small")
			animation_player.play("big_to_small")

		PlayerMode.FIRE:
			set_physics_process(false)


	# Tween for death and swap to death animation

func handle_flower_collision():
	pass

func handle_small_to_big():
	if (player_mode == PlayerMode.SMALL):
		player_mode = PlayerMode.BIG
		animated_sprite.play("big_idle")
	else:
		player_mode = PlayerMode.SMALL
		animated_sprite.play("small_idle")
		
	invincible = true
	invincibility_timer.start()

func _on_area_2d_area_entered(area:Area2D) -> void:
	if area is Mushroom:
		handle_mushroom_collision()
		area.queue_free()
	elif area is Enemy:
		handle_enemy_collision()
	# elif area is Flower:
		# handle_flower_collision()


	pass # Replace with function body.

func handle_death_tween():
	music.stop()
	var spawn_tween = get_tree().create_tween()
	spawn_tween.tween_property(self, "position", position + Vector2(0, -16), 0.4)
	spawn_tween.tween_property(self, "position", position + Vector2(0, 180), 0.4)
	spawn_tween.tween_property(self, "position", position + Vector2(0, 200), 4)

func handle_death():
	get_tree().reload_current_scene()


#func set_collision_shapes(is_small: bool):
	#var collision_shape = SMALL_MARIO_COLLISION_SHAPE if is_small else BIG_MARIO_COLLISION_SHAPE
	#area_2d.set_deferred("shape", collision_shape)
	#collisionShape2D.set_deferred("shape", collision_shape)


func _on_invincibility_timer_timeout() -> void:
	invincible = false # Replace with function body.
