extends AnimatedSprite2D


class_name PlayerAnimatedSprite
var frame_count = 0
# Called when the node enters the scene tree for the first time.
func trigger_animation(velocity: Vector2, direction: int, player_mode: Player.PlayerMode):
	# var animation_prefix = Player.PlayerMode.keys()[player_mode].to_snake_case()
	
	# if not get_parent().is_on_floor():
	# 	play("%s_jump" % animation_prefix)
	
	# #handle slide animations
	# elif sign(velocity.x) != sign(direction) && velocity.x != 0 && direction != 0:
	# 	play("%s_slide" % animation_prefix)
	# 	scale.x = direction
	# else:
	# # handle the sprite direction
	# 	if scale.x == 1 && sign(velocity.x) == -1:
	# 		scale.x = -1
	# 	elif scale.x == -1 && sign(velocity.x) == 1:
	# 		scale.x = 1
		
	# 	# handle run and idle animations
	# 	if velocity.x != 0:
	# 		play("%s_run" % animation_prefix)
	# 	else:
	# 		play("%s_idle" % animation_prefix)
	pass

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

func _on_animated_sprite_animation_finished() -> void:
	if animation == "small_to_big":
		reset_player_properties()
		#get_parent().player_mode = Player.PlayerMode.BIG if get_parent().player_mode == Player.PlayerMode.SMALL else Player.PlayerMode.SMALL



func reset_player_properties():
	offset = Vector2.ZERO
	get_parent().set_physics_process(true)
	get_parent().set_collision_layer_value(2, true)


func _on_frame_changed() -> void:
	if animation == "small_to_big":
		var player_mode = get_parent().player_mode
		frame_count += 1

		if frame_count % 2 == 1:
			offset = Vector2(0, 0 if player_mode == Player.PlayerMode.BIG else -6)
		else:
			offset = Vector2(0, 8 if player_mode == Player.PlayerMode.BIG else 0)


#func _on_animation_finished() -> void:
		#if animation == "small_to_big":
			#reset_player_properties() # Replace with function body.
