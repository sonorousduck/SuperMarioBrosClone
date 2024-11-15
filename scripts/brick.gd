extends StaticBody2D


class_name Brick

const COIN = preload("res://scenes/moving_coin.tscn")
const EMPTY_BOX = preload("res://assets/sprites/EmptyBox.png")

@onready var shape_cast = $RayCast2D as ShapeCast2D

@export var containing_item: Item.ItemType = Item.ItemType.NONE
@export var numCoins: int = 0

@onready var gpu_particles = $GPUParticles2D
@onready var sprite = $Sprite2D
@onready var collider = $CollisionShape2D
@onready var sfx = $AudioStreamPlayer2D
@onready var bump_sfx = $BumpSound

@onready var game_manager = %GameManager as GameManager

func tween_movement():
	var bump_tween = get_tree().create_tween()
	bump_tween.tween_property(self, "position", position + Vector2(0, -5), .12)
	bump_tween.chain().tween_property(self, "position", position, .12)
	bump_sfx.play()

func spawn_coin():
	var coin = COIN.instantiate()
	coin.global_position = global_position
	get_tree().root.add_child(coin)
	
func bump(player_mode):
	if (containing_item == Item.ItemType.COIN):
		if (numCoins > 0):
			spawn_coin()
			game_manager.add_coins()
			
			numCoins -= 1
			
			if (numCoins <= 0):
				sprite.texture = EMPTY_BOX
				sprite.region_enabled = false
			
			tween_movement()		
		
		else:
			bump_sfx.play()
			
		
		

	else:
		if (player_mode == Player.PlayerMode.BIG || player_mode == Player.PlayerMode.FIRE):
			# Play Animation
			sfx.playing = true
			gpu_particles.emitting = true
			sprite.visible = false
			set_collision_layer_value(1, false)
		else:
			var bump_tween = get_tree().create_tween()
			bump_tween.tween_property(self, "position", position + Vector2(0, -5), .12)
			bump_tween.chain().tween_property(self, "position", position, .12)
			bump_sfx.play()
	
		check_for_collision()
		
		
func check_for_collision():
	if shape_cast.is_colliding():
		for i in range(shape_cast.get_collision_count()):
			var collision = shape_cast.get_collider(i)

			if (collision is Goomba):
				var enemy = collision as Goomba
				enemy.die_from_hit()
			if (collision is Koopa):
				(collision as Koopa).die_from_hit()
			if (collision is Mushroom):
				var mushroom = collision as Mushroom
				mushroom.bump()




func _on_gpu_particles_2d_finished() -> void:
	queue_free()
