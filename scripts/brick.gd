extends StaticBody2D


class_name Brick



@onready var ray_cast_2d = $RayCast2D as RayCast2D

@export var containing_item: Item.ItemType = Item.ItemType.NONE
@export var numCoins: int = 0

func bump(player_mode):
	
	var bump_tween = get_tree().create_tween()
	bump_tween.tween_property(self, "position", position + Vector2(0, -5), .12)
	bump_tween.chain().tween_property(self, "position", position, .12)
	check_for_enemy_collision()
		
		
func check_for_enemy_collision():
	if ray_cast_2d.is_colliding() && ray_cast_2d.get_collider() is Enemy:
		var enemy = ray_cast_2d.get_collider() as Enemy
		enemy.die_from_hit()
