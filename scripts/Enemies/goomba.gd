extends CharacterBody2D

class_name Goomba
# Called when the node enters the scene tree for the first time.
# func _ready() -> void:
# 	pass # Replace with function body.

@export var SPEED: int = 60
var direction: int = -1
var isSquished: bool = false


@onready var raycast_right = $RayCastRight
@onready var raycast_left = $RayCastLeft
@onready var sprite = $Sprite2D as AnimatedSprite2D
@onready var animation_player = $AnimationPlayer
@onready var squished = $Area2D

func goomba_die():
	sprite.play("squished")


func handle_animation_player():
	isSquished = true
	animation_player.play("death")


func _ready() -> void:
	var squished = get_node("Area2D")
	squished.connect("squished", handle_animation_player)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if raycast_right.is_colliding():
		# var collider = raycast_right.get_collider()

		# if collider and collider.collision_layer & (1 << 2) != 0:
		direction = -1
		sprite.flip_h = true
	if raycast_left.is_colliding():
		# var collider = raycast_left.get_collider()

		# if collider and collider.collision_layer & (1 << 2) != 0:
		# 	direction = -1
		# 	sprite.flip_h = true
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
	if isSquished:
		return

	if not is_on_floor():
		velocity += get_gravity() * delta

	velocity.x = direction * SPEED
	move_and_slide()

func die_from_hit() -> void:
	queue_free()

	# for i in range(()):
	# 	var collision = get_slide_collision(i)



	# 	if (collision.has_method("bounce")):
	# 		print(collision)

	# 	if collision.get_normal().y > 0.7:
	# 		if collision.has_method("bounce"):
	# 			collision.bounce()
	# 			
