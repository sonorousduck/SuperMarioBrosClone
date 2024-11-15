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

func handle_non_squish_death():
	animation_player.play("death_by_shell")
	sprite.rotation = (deg_to_rad(180))
	var spawn_tween = get_tree().create_tween()
	spawn_tween.tween_property(self, "position", position + Vector2(0, -16), 0.2)
	spawn_tween.tween_property(self, "position", position + Vector2(0, 32), 0.2)


func _ready() -> void:
	squished.connect("squished", handle_animation_player)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if raycast_right.is_colliding():
		var collider = raycast_right.get_collider()
		if (collider is Player):
			(collider as Player).handle_enemy_collision()
		else:
			direction = -1
			sprite.flip_h = true
	if raycast_left.is_colliding():
		var collider = raycast_left.get_collider()
		if (collider is Player):
			(collider as Player).handle_enemy_collision()
		else:
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
	handle_non_squish_death()

	# for i in range(()):
	# 	var collision = get_slide_collision(i)



	# 	if (collision.has_method("bounce")):
	# 		print(collision)

	# 	if collision.get_normal().y > 0.7:
	# 		if collision.has_method("bounce"):
	# 			collision.bounce()
	# 			
