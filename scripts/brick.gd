extends StaticBody2D


class_name Brick



@onready var shape_cast = $RayCast2D as ShapeCast2D

@export var containing_item: Item.ItemType = Item.ItemType.NONE
@export var numCoins: int = 0

@onready var gpu_particles = $GPUParticles2D
@onready var sprite = $Sprite2D
@onready var collider = $CollisionShape2D
@onready var sfx = $AudioStreamPlayer2D

func bump(player_mode):
	if (player_mode == Player.PlayerMode.BIG || player_mode == Player.PlayerMode.FIRE):
		# Play Animation
		sfx.playing = true
		gpu_particles.emitting = true
		sprite.visible = false
	else:
		var bump_tween = get_tree().create_tween()
		bump_tween.tween_property(self, "position", position + Vector2(0, -5), .12)
		bump_tween.chain().tween_property(self, "position", position, .12)
	
	
	check_for_collision()
		
		
func check_for_collision():
	if shape_cast.is_colliding():
		for i in range(shape_cast.get_collision_count()):
			var collision = shape_cast.get_collider(i)
			print("COLLDING!")

			if (collision is Goomba):
				var enemy = collision as Goomba
				enemy.die_from_hit()
			if (collision is Mushroom):
				var mushroom = collision as Mushroom
				mushroom.bump()




func _on_gpu_particles_2d_finished() -> void:
	queue_free()

