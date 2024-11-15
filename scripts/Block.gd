extends StaticBody2D


class_name Block

const MUSHROOM = preload("res://scenes/Powerups/mushroom.tscn")
const COIN = preload("res://scenes/moving_coin.tscn")

@onready var shape_cast = $RayCast2D as ShapeCast2D

@export var containing_item: Item.ItemType = Item.ItemType.COIN;
@export var invisible: bool = false

@export var frame_count = 4;

@onready var animated_sprite = $Sprite
@onready var game_manager = %GameManager as GameManager
var startingFrame: int = 0
var is_empty = false;
# func _ready() -> void:
# 	# Set invisible

func _ready() -> void:
	startingFrame = randi_range(0, frame_count - 1)
	animated_sprite.set_frame_and_progress(startingFrame, 0.0)

func spawn_coin():
	var coin = COIN.instantiate()
	coin.global_position = global_position
	get_tree().root.add_child(coin)

func spawn_shroom():
	var mushroom = MUSHROOM.instantiate()
	mushroom.global_position = global_position
	get_tree().root.add_child(mushroom)

func make_empty():
	is_empty = true
	animated_sprite.play("become_empty")
	# Play animation to make it an empty block

func bump(player_mode):
	if is_empty:
		return

	var bump_tween = get_tree().create_tween()
	bump_tween.tween_property(self, "position", position + Vector2(0, -5), .12)
	bump_tween.chain().tween_property(self, "position", position, .12)
	check_for_collision()


	match containing_item:
		Item.ItemType.COIN:
			spawn_coin()
			game_manager.add_coins()
			
		Item.ItemType.MUSHROOM:
			spawn_shroom()
		Item.ItemType.ONE_UP:
			pass
		Item.ItemType.FIRE:
			pass
	
	make_empty()

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
