extends CharacterBody2D

const SPEED = 300.0
const SPRINT_MULTIPLIER = 1.5
const JUMP_VELOCITY = -400.0
const MAX_JUMPS = 2

var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")
var jumps_left = 1
var has_double_jump = false

func _physics_process(delta):
	# Add the gravity.
	if not is_on_floor():
		velocity.y += gravity * delta
	else:
		jumps_left = 1 if not has_double_jump else MAX_JUMPS

	# Handle Jump and Double Jump.
	if Input.is_action_just_pressed("ui_accept") and jumps_left > 0:
		velocity.y = JUMP_VELOCITY
		jumps_left -= 1

	# Get the input direction and handle the movement/deceleration.
	var direction = Input.get_axis("ui_left", "ui_right")
	var current_speed = SPEED * (SPRINT_MULTIPLIER if Input.is_action_pressed("ui_select") else 1.0)
	
	if direction:
		velocity.x = direction * current_speed
	else:
		velocity.x = move_toward(velocity.x, 0, current_speed)

	move_and_slide()

func collect_double_jump():
	has_double_jump = true
	jumps_left = MAX_JUMPS