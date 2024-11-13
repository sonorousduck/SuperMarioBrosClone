extends CharacterBody2D

class_name Player

const SPEED = 130.0
const GRAVITY_MODIFIER = 2
const JUMP_VELOCITY = -500.0 
const BOUNCE_VELOCITY = -200.0 

enum PlayerMode {
	SMALL,
	BIG,
	SHOOTING
}

var player_mode = PlayerMode.SMALL

func handle_movement_collision(collision: KinematicCollision2D):
	if (collision.get_collider() is Block):
		var collision_angle = rad_to_deg(collision.get_angle())
		if roundf(collision_angle) == 180:
			(collision.get_collider() as Block).bump(player_mode)
		pass
	
	# if (collision.get_collider() is Pipe):
	# 	pass

# This function is used when the player kills an enemy
func bounce():
	velocity.y = BOUNCE_VELOCITY
	# move_and_slide()


func _physics_process(delta: float) -> void:
	# Add the gravity.
	if not is_on_floor():
		velocity += get_gravity() * delta * GRAVITY_MODIFIER
	

	if Input.is_action_just_pressed("esc"):
		get_tree().quit()


	# Handle jump.
	if Input.is_action_just_pressed("jump") and is_on_floor():
		velocity.y = JUMP_VELOCITY

	if Input.is_action_just_released("jump") and velocity.y < 0:
		velocity.y *= 0.5;

	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	var direction := Input.get_axis("move_left", "move_right")
	if direction:
		velocity.x = direction * SPEED
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)


	var collision = get_last_slide_collision()
	if collision != null:
		handle_movement_collision(collision)



	move_and_slide()
