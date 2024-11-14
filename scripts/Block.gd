extends StaticBody2D


class_name Block

const MUSHROOM = preload("res://scenes/Powerups/mushroom.tscn")

@onready var ray_cast_2d = $RayCast2D as RayCast2D

@export var containing_item: Item.ItemType = Item.ItemType.COIN;
@export var invisible: bool = false

@export var frame_count = 4;

@onready var animated_sprite = $Sprite
var startingFrame: int = 0
var is_empty = false;
# func _ready() -> void:
# 	# Set invisible

func _ready() -> void:
	startingFrame = randi_range(0, frame_count - 1)
	animated_sprite.set_frame_and_progress(startingFrame, 0.0)



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
	check_for_enemy_collision()


	match containing_item:
		Item.ItemType.COIN:
			pass
		Item.ItemType.MUSHROOM:
			spawn_shroom()
		Item.ItemType.ONE_UP:
			pass
		Item.ItemType.FIRE:
			pass
	
	make_empty()

func check_for_enemy_collision():
	if ray_cast_2d.is_colliding() && ray_cast_2d.get_collider() is Enemy:
		var enemy = ray_cast_2d.get_collider() as Enemy
		enemy.die_from_hit()
